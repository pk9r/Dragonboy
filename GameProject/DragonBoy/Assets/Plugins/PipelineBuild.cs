#if UNITY_EDITOR
using System;
using System.IO;
using Mono.Cecil.Cil;
using Mono.Cecil;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;

public class PipelineBuild : IPostBuildPlayerScriptDLLs
{
    public int callbackOrder => 0;

    public void OnPostBuildPlayerScriptDLLs(BuildReport report)
    {
        if (BuildPipeline.isBuildingPlayer && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            EditorApplication.LockReloadAssemblies();
            string path = GetAssemblyLocation("Assembly-CSharp.dll");
            if (File.Exists(path))
                ModifyInstallAll(path);
            EditorApplication.UnlockReloadAssemblies();
        }
    }

    /// <summary>
    /// Tối ưu mã IL của hàm <see cref="Mod.GameEventHook.InstallAll"/>.
    /// </summary>
    /// <param name="assemblyCSharpPath">Đường dẫn tệp Asembly-CSharp.dll</param>
    /// <remarks>
    /// Mã IL được tạo sẽ tương tự như sau:
    /// <code>
    /// ldtoken     [target method]
    /// call        class System.Reflection.MethodBase System.Reflection.MethodBase::GetMethodFromHandle(valuetype System.RuntimeMethodHandle)
    /// ldtoken     [hook method]
    /// call        class System.Reflection.MethodBase System.Reflection.MethodBase::GetMethodFromHandle(valuetype System.RuntimeMethodHandle)
    /// ldtoken     [trampoline method]
    /// call        class System.Reflection.MethodBase System.Reflection.MethodBase::GetMethodFromHandle(valuetype System.RuntimeMethodHandle)
    /// call        void Mod.GameEventHook::TryInstallHook(class System.Reflection.MethodBase, class System.Reflection.MethodBase, class System.Reflection.MethodBase)
    /// ...
    /// ret
    /// </code>
    /// </remarks>
    static void ModifyInstallAll(string assemblyCSharpPath)
    {
        byte[] data = File.ReadAllBytes(assemblyCSharpPath);

        AssemblyDefinition assemblyCSharp = AssemblyDefinition.ReadAssembly(new MemoryStream(data));
        ((DefaultAssemblyResolver)assemblyCSharp.MainModule.AssemblyResolver).AddSearchDirectory(Path.GetDirectoryName(assemblyCSharpPath));
        TypeDefinition gameEventHook = assemblyCSharp.MainModule.GetType("Mod.GameEventHook");
        MethodDefinition installAll = gameEventHook.Methods.First(m => m.Name == "InstallAll");
        TypeReference methodBase = new TypeReference("System.Reflection", nameof(MethodBase), assemblyCSharp.MainModule, assemblyCSharp.MainModule.TypeSystem.CoreLibrary);
        TypeReference runtimeMethodHandle = new TypeReference("System", nameof(RuntimeMethodHandle), assemblyCSharp.MainModule, assemblyCSharp.MainModule.TypeSystem.CoreLibrary, true);
        MethodReference getMethodFromHandle = new MethodReference(nameof(MethodBase.GetMethodFromHandle), methodBase, methodBase)
        {
            HasThis = false,
            ExplicitThis = false,
            CallingConvention = MethodCallingConvention.Default,
        };
        getMethodFromHandle.Parameters.Add(new ParameterDefinition("handle", ParameterAttributes.None, runtimeMethodHandle));

        //TryInstallHook<T1, T2>(T1 hookTargetMethod, T2 hookMethod, T2 originalProxyMethod)
        MethodDefinition tryInstallHookGeneric_withTrampoline = gameEventHook.Methods.First(m => m.Name == "TryInstallHook" && m.GenericParameters.Count == 2 && m.Parameters.Count == 3);
        //TryInstallHook<T1, T2>(T1 hookTargetMethod, T2 hookMethod)
        MethodDefinition tryInstallHookGeneric = gameEventHook.Methods.First(m => m.Name == "TryInstallHook" && m.GenericParameters.Count == 2 && m.Parameters.Count == 2);

        //TryInstallHook(MethodBase hookTargetMethod, MethodBase hookMethod, MethodBase originalProxyMethod)
        MethodDefinition tryInstallHook = gameEventHook.Methods.First(m => m.Name == "TryInstallHook" && m.GenericParameters.Count == 0);

        List<Instruction> instructions = new List<Instruction>();
        int paramCount = 0;
        for (int i = 0; i < installAll.Body.Instructions.Count; i++)
        {
            Instruction instruction = installAll.Body.Instructions[i];
            if (instruction.OpCode == OpCodes.Ldftn)
            {
                //ldftn     <target/hook/trampoline method>
                if (instruction.Operand is MethodDefinition methodDef)
                {
                    instructions.Add(Instruction.Create(OpCodes.Ldtoken, methodDef));
                    instructions.Add(Instruction.Create(OpCodes.Call, getMethodFromHandle));
                    //MethodBase.GetMethodFromHandle(methodof(<some method here>).MethodHandle
                    paramCount++;
                }
            }
            else if (instruction.OpCode == OpCodes.Ldvirtftn)
            {
                //ldvirtftn     <target method (virtual)>
                if (i >= 2 && instruction.Operand is MethodDefinition methodDef)
                {
                    Instruction instruction1 = installAll.Body.Instructions[i - 1];
                    Instruction instruction2 = installAll.Body.Instructions[i - 2];
                    if (instruction1.OpCode == OpCodes.Dup && instruction2.IsLdloc())
                    {
                        VariableDefinition local = installAll.Body.Variables[instruction2.GetLdlocIndex()];
                        instructions.Add(Instruction.Create(OpCodes.Ldtoken, local.VariableType.Resolve().Methods.First(m => m.Name == methodDef.Name)));
                        instructions.Add(Instruction.Create(OpCodes.Call, getMethodFromHandle));
                        //MethodBase.GetMethodFromHandle(methodof(<some method here>).MethodHandle
                        paramCount++;
                    }
                }
            }
            else if (instruction.OpCode == OpCodes.Ldtoken)
            {
                //ldtoken       <some type>
                if (i + 4 < installAll.Body.Instructions.Count && instruction.Operand is TypeDefinition typeToGetConstructor)
                {
                    Instruction instruction2 = installAll.Body.Instructions[i + 1];
                    Instruction instruction3 = installAll.Body.Instructions[i + 2];
                    Instruction instruction4 = installAll.Body.Instructions[i + 3];
                    Instruction instruction5 = installAll.Body.Instructions[i + 4];
                    if (instruction2.OpCode == OpCodes.Call && instruction2.Operand is MethodReference getTypeFromHandle && getTypeFromHandle.Name == nameof(Type.GetTypeFromHandle))
                    {
                        //typeof(<some type>).GetConstructor(new Type[<some number>] {typeof(...), ... })
                        if (instruction3.IsLdcI4() &&
                            instruction4.OpCode == OpCodes.Newarr && instruction4.Operand is TypeReference typeReference2 && typeReference2.Name == nameof(Type))
                        {
                            //ldc.i4     <number of types>
                            //newarr     [System.Type]
                            //...
                            //dup
                            //ldc.i4     <index>
                            //ldtoken    <some type>
                            //call       class System.Type System.Type::GetTypeFromHandle(valuetype System.RuntimeTypeHandle)
                            //stelem.ref
                            //...
                            //call       class System.Reflection.ConstructorInfo System.Type::GetConstructor(class System.Type[])
                            int typesCount = instruction3.GetLdcI4Value();
                            TypeReference[] paramTypes = new TypeReference[typesCount];
                            for (int j = 0; j < typesCount; j++)
                            {
                                int index = installAll.Body.Instructions[i + 5 * (j + 1)].GetLdcI4Value();
                                Instruction ldTokenTypeParam = installAll.Body.Instructions[i + 5 * (j + 1) + 1];
                                if (ldTokenTypeParam.Operand is TypeReference typeRefParam)
                                    paramTypes[index] = typeRefParam;
                            }
                            MethodDefinition _ctor = typeToGetConstructor.Methods.First(m => m.IsConstructor && m.Parameters.Select(p => p.ParameterType.FullName).SequenceEqual(paramTypes.Select(p => p.FullName)));
                            instructions.Add(Instruction.Create(OpCodes.Ldtoken, _ctor));
                            instructions.Add(Instruction.Create(OpCodes.Call, getMethodFromHandle));
                            //MethodBase.GetMethodFromHandle(methodof(<constructor (.ctor) with x parameters>).MethodHandle
                            paramCount++;
                        }
                        //typeof(<some type>).GetConstructors()[0]
                        else if (instruction3.OpCode == OpCodes.Call && instruction3.Operand is MethodReference getConstructors && getConstructors.Name == nameof(Type.GetConstructors) &&
                            instruction4.OpCode == OpCodes.Ldc_I4_0 &&
                            instruction5.OpCode == OpCodes.Ldelem_Ref)
                        {
                            instructions.Add(Instruction.Create(OpCodes.Ldtoken, typeToGetConstructor.Methods.First(m => m.IsConstructor)));
                            instructions.Add(Instruction.Create(OpCodes.Call, getMethodFromHandle));
                            //MethodBase.GetMethodFromHandle(methodof(<constructor (.ctor)>).MethodHandle
                            paramCount++;
                        }
                    }
                }
            }
            else if (instruction.OpCode == OpCodes.Call)
            {
                if (instruction.Operand is MethodSpecification methodSpec)
                {
                    //call      TryInstallHook<T1, T2>([target method], [hook method], [trampoline method])
                    if (methodSpec.Resolve() == tryInstallHookGeneric_withTrampoline && paramCount == 3)
                    {
                        instructions.Add(Instruction.Create(OpCodes.Call, tryInstallHook));
                        paramCount = 0;
                    }
                    //call      TryInstallHook<T1, T2>([target method], [hook method])
                    else if (methodSpec.Resolve() == tryInstallHookGeneric && paramCount == 2)
                    {
                        while (paramCount < 3)
                        {
                            instructions.Add(Instruction.Create(OpCodes.Ldnull));
                            paramCount++;
                        }
                        instructions.Add(Instruction.Create(OpCodes.Call, tryInstallHook));
                        paramCount = 0;
                    }
                }
                //call      TryInstallHook(.....)
                else if (instruction.Operand is MethodDefinition methodDefinition && methodDefinition == tryInstallHook)
                {
                    //trampoline method can be null
                    while (paramCount < 3)
                    {
                        instructions.Add(Instruction.Create(OpCodes.Ldnull));
                        paramCount++;
                    }
                    instructions.Add(Instruction.Create(OpCodes.Call, tryInstallHook));
                    paramCount = 0;
                }
            }
            //The final result will be:
            //GameEventHook.TryInstallHook(MethodBase.GetMethodFromHandle(methodof(<some method/constructor (target)>).MethodHandle), MethodBase.GetMethodFromHandle(methodof(<some method/constructor (hook)>).MethodHandle), MethodBase.GetMethodFromHandle(methodof(<some method/constructor (trampoline)>).MethodHandle));
        }
        instructions.Add(Instruction.Create(OpCodes.Ret));
        installAll.Body.Instructions.Clear();
        instructions.ForEach(i => installAll.Body.Instructions.Add(i));

        assemblyCSharp.Write(assemblyCSharpPath);
    }

    static string GetAssemblyLocation(string assembly)
    {
        if (File.Exists(assembly) && assembly.EndsWith(".dll"))
            return assembly;
        if (!assembly.EndsWith(".dll"))
            assembly += ".dll";
        if (GetAssemblyLocationInTempStagingArea(assembly, out string path))
            return path;
        return null;
    }

    static bool GetAssemblyLocationInTempStagingArea(string assemblyName, out string assemblyLocation)
    {
        string asmPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Temp", "StagingArea", "Data", "Managed", assemblyName);
        if (File.Exists(asmPath))
        {
            assemblyLocation = asmPath;
            return true;
        }
        assemblyLocation = null;
        return false;
    }
}

internal static class ExtensionMethods
{
    internal static bool IsLdcI4(this Instruction instruction) => instruction.OpCode.Code - 21 <= Code.Stloc_1;

    internal static bool IsLdloc(this Instruction instruction) => instruction.OpCode == OpCodes.Ldloc || instruction.OpCode == OpCodes.Ldloc_0 || instruction.OpCode == OpCodes.Ldloc_1 || instruction.OpCode == OpCodes.Ldloc_2 || instruction.OpCode == OpCodes.Ldloc_3 || instruction.OpCode == OpCodes.Ldloc_S;

    internal static int GetLdcI4Value(this Instruction instruction)
    {
        return instruction.OpCode.Code switch
        {
            Code.Ldc_I4_M1 => -1,
            Code.Ldc_I4_0 => 0,
            Code.Ldc_I4_1 => 1,
            Code.Ldc_I4_2 => 2,
            Code.Ldc_I4_3 => 3,
            Code.Ldc_I4_4 => 4,
            Code.Ldc_I4_5 => 5,
            Code.Ldc_I4_6 => 6,
            Code.Ldc_I4_7 => 7,
            Code.Ldc_I4_8 => 8,
            Code.Ldc_I4_S => (sbyte)instruction.Operand,
            Code.Ldc_I4 => (int)instruction.Operand,
            _ => throw new InvalidOperationException($"Not a ldc.i4 instruction: {instruction}"),
        };
    }

    internal static int GetLdlocIndex(this Instruction instruction)
    {
        switch (instruction.OpCode.Code)
        {
            case Code.Ldloc_0:
                return 0;
            case Code.Ldloc_1:
                return 1;
            case Code.Ldloc_2:
                return 2;
            case Code.Ldloc_3:
                return 3;
            case Code.Ldloc_S:
            case Code.Ldloc:
                return ((VariableReference)instruction.Operand).Index;
            default:
                throw new InvalidOperationException($"Not a ldloc instruction: {instruction}");
        }
    }
}
#endif