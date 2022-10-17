using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Mod
{
    public class Extension
    {
        public static List<Extension> Extensions { get; private set; } = new List<Extension>();

        public string ExtensionName { get; private set; }

        public string ExtensionDescription { get; private set; }

        public string ExtensionVersion { get; private set; }

        bool hasMenuItems;

        const BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod;

        Assembly extensionAssembly;

        public Extension(string path) 
        {
            extensionAssembly = Assembly.LoadFrom(path);
            extensionAssembly.GetType("Loader").GetMethod("Init", flags).Invoke(null, null);
            FieldInfo name = extensionAssembly.GetType("MainExt").GetField("name", BindingFlags.Public | BindingFlags.Static);
            if (name != null)
                ExtensionName = (string)name.GetRawConstantValue();
            FieldInfo desc = extensionAssembly.GetType("MainExt").GetField("description", BindingFlags.Public | BindingFlags.Static);
            if (desc != null)
                ExtensionDescription = (string)desc.GetRawConstantValue();
            FieldInfo ver = extensionAssembly.GetType("MainExt").GetField("version", BindingFlags.Public | BindingFlags.Static);
            if (ver != null)
                ExtensionVersion = (string)ver.GetRawConstantValue();
            if (string.IsNullOrEmpty(ExtensionName))
                ExtensionName = extensionAssembly.FullName;
            if (string.IsNullOrEmpty(ExtensionDescription))
                ExtensionDescription = extensionAssembly.ManifestModule.Name;
            if (string.IsNullOrEmpty(ExtensionVersion))
                ExtensionVersion = extensionAssembly.GetName().Version.ToString();
            hasMenuItems = extensionAssembly.GetType("MainExt") != null && extensionAssembly.GetType("MainExt").GetMethod("OpenMenu", flags) != null;
        }

        public bool HasMenuItems()
        {
            return hasMenuItems;
        }

        public void OpenMenu()
        {
            if (hasMenuItems) 
                extensionAssembly.GetType("MainExt").GetMethod("OpenMenu", flags).Invoke(null, null);
        }

        public static void LoadExtensions()
        {
            string extensionDir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))) + "\\Extensions";
            if (!Directory.Exists(extensionDir))
                Directory.CreateDirectory(extensionDir);
            foreach (string path in Directory.GetFiles(extensionDir))
            {
                try
                {
                    Extensions.Add(new Extension(path));
                }
                catch(Exception ex)
                {
                    Debug.LogError("Exception when loading extension module: " + path);
                    Debug.LogException(ex);
                }
            }
        }
    }
}
