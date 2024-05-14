using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace AssemblyCSharpPreprocessor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: AssemblyCSharpPreprocessor <input file> [<output file>] [options]");
                Console.WriteLine("Options: ");
                Console.WriteLine("\t-d, --delete-unused-classes: delete unused classes (iOSPlugins, SMS)");
                Console.WriteLine("\t-r, --rename-classes: rename classes (Math => Math2)");
                Console.WriteLine("\t-c, --change-all-private-members-to-internal: change all access modifier of private members (types, methods, fields, properties, events) to internal");
                Console.WriteLine("\t-o, --overwrite-file: overwrite input file");
                Environment.Exit(1);
                return;
            }
            string inputFile = args[0];
            CommandLineOptions options = CommandLineOptions.Parse(args);
            args = args.Where(s => !s.StartsWith("-")).ToArray();
            string outputFile = args.Length > 1 ? args[1] : null;
            byte[] data = File.ReadAllBytes(inputFile);
            ModuleDefMD assemblyCSharp = ModuleDefMD.Load(data);
            if (assemblyCSharp.Assembly.Name != "Assembly-CSharp")
            {
                Console.WriteLine("Input file is not Assembly-CSharp!");
                return;
            }
            if (options.DeleteUnusedClasses)
                DeleteUnusedClasses(assemblyCSharp);
            if (options.RenameClasses)
                RenameClasses(assemblyCSharp);
            if (options.ChangeAllPrivateMembersToInternal)
                ChangeAllPrivateMembersToInternal(assemblyCSharp);
            try
            {
                if (outputFile != null)
                {
                    if (options.OverwriteInputFile)
                        Console.WriteLine("Not overwriting original file!");
                    assemblyCSharp.Write(outputFile);
                }
                else if (options.OverwriteInputFile)
                    assemblyCSharp.Write(inputFile);
                else
                    assemblyCSharp.Write(Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + "-processed" + Path.GetExtension(inputFile)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (!File.Exists(inputFile))
                    File.WriteAllBytes(inputFile, data);
            }
            if (outputFile == null && options.OverwriteInputFile)
                File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(inputFile), Path.GetFileNameWithoutExtension(inputFile) + "-original" + Path.GetExtension(inputFile)), data);
        }

        static void RenameClasses(ModuleDefMD assemblyCSharp)
        {
            Console.WriteLine("Renaming classes...");
            assemblyCSharp.Find("Math", false).Name = "Math2";
        }

        static void DeleteUnusedClasses(ModuleDefMD assemblyCSharp)
        {
            Console.WriteLine("Deleting unused classes...");
            MethodDef fixedUpdate = assemblyCSharp.Find("Main", false).FindMethod("FixedUpdate");
            for (int i = fixedUpdate.Body.Instructions.Count - 1; i >= 0; i--)
            {
                Instruction instruction = fixedUpdate.Body.Instructions[i];
                if (instruction.OpCode != OpCodes.Call)
                    continue;
                if (instruction.Operand is MethodDef method && method.DeclaringType.FullName == "SMS")
                    fixedUpdate.Body.Instructions.RemoveAt(i);
            }
            TypeDef sms = assemblyCSharp.GetTypes().FirstOrDefault(t => t.FullName == "SMS");
            if (sms != null)
                assemblyCSharp.Types.Remove(sms);
            TypeDef iOSPlugins = assemblyCSharp.GetTypes().FirstOrDefault(t => t.FullName == "iOSPlugins");
            if (iOSPlugins != null)
                assemblyCSharp.Types.Remove(iOSPlugins);
        }

        static void ChangeAllPrivateMembersToInternal(ModuleDefMD assemblyCSharp)
        {
            Console.WriteLine("Changing access modifiers...");
            foreach (TypeDef type in assemblyCSharp.GetTypes().Where(t => t.GetClosestCompilerGeneratedAttribute() == null))
            {
                foreach (MethodDef method in type.Methods)
                {
                    if (method.IsPrivate)
                        method.Access = MethodAttributes.Assembly;
                }
                foreach (FieldDef field in type.Fields)
                {
                    if (field.IsPrivate)
                        field.Access = FieldAttributes.Assembly;
                }
                foreach (PropertyDef property in type.Properties)
                {
                    if (property.GetMethod != null && property.GetMethod.IsPrivate)
                        property.GetMethod.Access = MethodAttributes.Assembly;
                    if (property.SetMethod != null && property.SetMethod.IsPrivate)
                        property.SetMethod.Access = MethodAttributes.Assembly;
                    if (property.OtherMethods != null)
                    {
                        foreach (MethodDef method in property.OtherMethods)
                        {
                            if (method.IsPrivate)
                                method.Access = MethodAttributes.Assembly;
                        }
                    }
                }
                foreach (EventDef @event in type.Events)
                {
                    if (@event.AddMethod != null && @event.AddMethod.IsPrivate)
                        @event.AddMethod.Access = MethodAttributes.Assembly;
                    if (@event.RemoveMethod != null && @event.RemoveMethod.IsPrivate)
                        @event.RemoveMethod.Access = MethodAttributes.Assembly;
                    if (@event.InvokeMethod != null && @event.InvokeMethod.IsPrivate)
                        @event.InvokeMethod.Access = MethodAttributes.Assembly;
                    if (@event.OtherMethods != null)
                    {
                        foreach (MethodDef method in @event.OtherMethods)
                        {
                            if (method.IsPrivate)
                                method.Access = MethodAttributes.Assembly;
                        }
                    }
                }
            }
        }
    }
}
