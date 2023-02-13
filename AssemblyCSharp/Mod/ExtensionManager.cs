using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Mod
{
    internal class ExtensionManager
    {
        internal static List<ExtensionManager> Extensions { get; private set; } = new List<ExtensionManager>();

        internal string ExtensionName { get; private set; }

        internal string ExtensionDescription { get; private set; }

        internal string ExtensionVersion { get; private set; }

        MemberInfo singletonFieldOrMethod;

        bool hasMenuItems;

        bool isOverrideExtensionClass;

        const BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod;

        Assembly extensionAssembly;

        internal ExtensionManager(string path) 
        {
            extensionAssembly = Assembly.LoadFrom(path);
            Type loader = extensionAssembly.GetType("Loader") ?? throw new NotAnExtensionException();
            MethodInfo init = loader.GetMethod("Init", flags) ?? throw new NotAnExtensionException();
            init.Invoke(null, null);
            Type mainExt = extensionAssembly.GetType("MainExt") ?? throw new NotAnExtensionException();
            FieldInfo name = mainExt.GetField("name", BindingFlags.Public | BindingFlags.Static);
            if (name != null)
                ExtensionName = (string)name.GetRawConstantValue();
            FieldInfo desc = mainExt.GetField("description", BindingFlags.Public | BindingFlags.Static);
            if (desc != null)
                ExtensionDescription = (string)desc.GetRawConstantValue();
            FieldInfo ver = mainExt.GetField("version", BindingFlags.Public | BindingFlags.Static);
            if (ver != null)
                ExtensionVersion = (string)ver.GetRawConstantValue();
            if (string.IsNullOrEmpty(ExtensionName))
                ExtensionName = extensionAssembly.FullName;
            if (string.IsNullOrEmpty(ExtensionDescription))
                ExtensionDescription = extensionAssembly.ManifestModule.Name;
            if (string.IsNullOrEmpty(ExtensionVersion))
                ExtensionVersion = extensionAssembly.GetName().Version.ToString();
            hasMenuItems = mainExt.GetMethod("OpenMenu", flags) != null;
            isOverrideExtensionClass = extensionAssembly.GetType("GameEvents") != null && extensionAssembly.GetType("GameEvents").IsSubclassOf(typeof(Extension));
            if (isOverrideExtensionClass)
            {
                try
                {
                    singletonFieldOrMethod = extensionAssembly.GetType("GameEvents").GetMethods(flags).First((method) => method.Name == "gI" || method.Name.ToLower() == "getinstance" || method.ReturnType == extensionAssembly.GetType("GameEvents"));
                }
                catch (Exception)
                {
                    try
                    {
                        singletonFieldOrMethod = extensionAssembly.GetType("GameEvents").GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).First((field) => field.Name.ToLower() == "_instance" || field.Name.ToLower() == "instance" || field.FieldType == extensionAssembly.GetType("GameEvents"));
                    }
                    catch (Exception)
                    {
                        UnityEngine.Debug.LogError("Could not find Singleton method or field!");
                        throw;
                    }
                }
                if (singletonFieldOrMethod == null)
                {
                    UnityEngine.Debug.LogError("Could not find Singleton method or field!");
                    throw new NullReferenceException(nameof(singletonFieldOrMethod));
                }
            }
        }

        internal bool HasMenuItems()
        {
            return hasMenuItems;
        }

        internal void OpenMenu()
        {
            if (hasMenuItems) 
                extensionAssembly.GetType("MainExt").GetMethod("OpenMenu", flags).Invoke(null, null);
        }

        internal static void Invoke(params object[] parameters)
        {
            foreach (ExtensionManager extensionManager in Extensions)
                if (extensionManager.isOverrideExtensionClass)
                    extensionManager.TryInvoke<object>(parameters, out _);
        }

        internal bool TryInvoke<T>(object[] parameters, out T result)
        {
            if (new StackFrame(2).GetMethod().DeclaringType != typeof(GameEvents))
                throw new MethodAccessException();
            try
            {
                object instance = null;
                if (singletonFieldOrMethod is FieldInfo fieldInfo)
                    instance = fieldInfo.GetValue(null);
                else if (singletonFieldOrMethod is MethodInfo methodInfo)
                    instance = methodInfo.Invoke(null, null);
                result = (T)extensionAssembly.GetType("GameEvents").GetMethod(new StackFrame(2).GetMethod().Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod).Invoke(instance, parameters);
            }
            catch (TargetInvocationException tEx)
            {
                result = default;
                if (tEx.InnerException != null && tEx.InnerException is NotImplementedException)
                    return false;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ExtensionName + ": Cannot invoke method: " + new StackFrame(2).GetMethod().Name + "!");
                UnityEngine.Debug.LogException(ex);
                result = default;
                return false;
            }
            return true;
        }

        internal static void LoadExtensions()
        {
            string extensionDir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))) + "\\Extensions";
            if (!Directory.Exists(extensionDir))
                Directory.CreateDirectory(extensionDir);
            foreach (string path in Directory.GetFiles(extensionDir).Where(p => Path.GetExtension(p) == ".dll"))
            {
                try
                {
                    Extensions.Add(new ExtensionManager(path));
                }
                catch (NotAnExtensionException) { }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError("Exception when loading extension module: " + path);
                    UnityEngine.Debug.LogException(ex);
                }
            }
        }
    }
}
