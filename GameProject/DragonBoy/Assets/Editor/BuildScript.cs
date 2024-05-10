
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace UnityBuilderAction
{
    enum MyBuildTarget
    {
        StandaloneWindowsX86,
        StandaloneWindowsX86_64,
        StandaloneWindowsArm64,
        Android,
        StandaloneLinuxX86_64,
    }

    public static class BuildScript
    {
        static readonly string Eol = Environment.NewLine;

        static readonly string[] Secrets =
            {"androidKeystorePass", "androidKeyaliasName", "androidKeyaliasPass"};

        public static void Build()
        {
#if UNITY_STANDALONE_WIN
            UnityEditor.WindowsStandalone.UserBuildSettings.copyPDBFiles = true;
#endif
            // Gather values from args
            Dictionary<string, string> options = GetValidatedOptions();
            MyBuildTarget buildTarget = Enum.Parse<MyBuildTarget>(options["buildTarget"]);

            ScriptingImplementation scriptingBackend = 0;
            if (options.ContainsKey("scriptingBackend"))
                scriptingBackend = Enum.Parse<ScriptingImplementation>(options["scriptingBackend"]);
            PlayerSettings.SetScriptingBackend(GetNamedBuildTarget(buildTarget), scriptingBackend);

            // Apply build target
            switch (buildTarget)
            {
                case MyBuildTarget.Android:
                {
                    PlayerSettings.bundleVersion = options["buildVersion"];
                    PlayerSettings.Android.bundleVersionCode = int.Parse(options["androidVersionCode"]);
                    EditorUserBuildSettings.buildAppBundle = options["customBuildPath"].EndsWith(".aab");
                    if (options.TryGetValue("androidKeystoreName", out string keystoreName) &&
                        !string.IsNullOrEmpty(keystoreName))
                    {
                        PlayerSettings.Android.useCustomKeystore = true;
                        PlayerSettings.Android.keystoreName = keystoreName;
                    }
                    if (options.TryGetValue("androidKeystorePass", out string keystorePass) &&
                        !string.IsNullOrEmpty(keystorePass))
                        PlayerSettings.Android.keystorePass = keystorePass;
                    if (options.TryGetValue("androidKeyaliasName", out string keyaliasName) &&
                        !string.IsNullOrEmpty(keyaliasName))
                        PlayerSettings.Android.keyaliasName = keyaliasName;
                    if (options.TryGetValue("androidKeyaliasPass", out string keyaliasPass) &&
                        !string.IsNullOrEmpty(keyaliasPass))
                        PlayerSettings.Android.keyaliasPass = keyaliasPass;
                    if (options.TryGetValue("androidTargetSdkVersion", out string androidTargetSdkVersion) &&
                        !string.IsNullOrEmpty(androidTargetSdkVersion))
                    {
                        var targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
                        try
                        {
                            targetSdkVersion = Enum.Parse<AndroidSdkVersions>(androidTargetSdkVersion);
                        }
                        catch
                        {
                            UnityEngine.Debug.Log("Failed to parse androidTargetSdkVersion! Fallback to AndroidApiLevelAuto");
                        }

                        PlayerSettings.Android.targetSdkVersion = targetSdkVersion;
                    }

                    break;
                }
            }
            // Determine subtarget
            int buildSubtarget = 0;
#if UNITY_2021_2_OR_NEWER
            if (!options.TryGetValue("standaloneBuildSubtarget", out var subtargetValue) || !Enum.TryParse(subtargetValue, out StandaloneBuildSubtarget buildSubtargetValue))
            {
                buildSubtargetValue = default;
            }
            buildSubtarget = (int)buildSubtargetValue;
#endif
            // Custom build
            Build(buildTarget, buildSubtarget, options["customBuildPath"]);
        }

        static Dictionary<string, string> GetValidatedOptions()
        {
            ParseCommandLineArguments(out Dictionary<string, string> validatedOptions);

            if (!validatedOptions.TryGetValue("projectPath", out string _))
            {
                Console.WriteLine("Missing argument -projectPath");
                EditorApplication.Exit(110);
            }

            if (!validatedOptions.TryGetValue("buildTarget", out string buildTarget))
            {
                Console.WriteLine("Missing argument -buildTarget");
                EditorApplication.Exit(120);
            }

            if (!Enum.IsDefined(typeof(MyBuildTarget), buildTarget ?? string.Empty))
            {
                Console.WriteLine($"{buildTarget} is not a defined {nameof(MyBuildTarget)}");
                EditorApplication.Exit(121);
            }

            if (!validatedOptions.TryGetValue("customBuildPath", out string _))
            {
                Console.WriteLine("Missing argument -customBuildPath");
                EditorApplication.Exit(130);
            }

            const string defaultCustomBuildName = "TestBuild";
            if (!validatedOptions.TryGetValue("customBuildName", out string customBuildName))
            {
                Console.WriteLine($"Missing argument -customBuildName, defaulting to {defaultCustomBuildName}.");
                validatedOptions.Add("customBuildName", defaultCustomBuildName);
            }
            else if (customBuildName == "")
            {
                Console.WriteLine($"Invalid argument -customBuildName, defaulting to {defaultCustomBuildName}.");
                validatedOptions.Add("customBuildName", defaultCustomBuildName);
            }

            return validatedOptions;
        }

        static void ParseCommandLineArguments(out Dictionary<string, string> providedArguments)
        {
            providedArguments = new Dictionary<string, string>();
            string[] args = Environment.GetCommandLineArgs();

            Console.WriteLine(
                $"{Eol}" +
                $"###########################{Eol}" +
                $"#    Parsing settings     #{Eol}" +
                $"###########################{Eol}" +
                $"{Eol}"
            );

            // Extract flags with optional values
            for (int current = 0, next = 1; current < args.Length; current++, next++)
            {
                // Parse flag
                bool isFlag = args[current].StartsWith("-");
                if (!isFlag) continue;
                string flag = args[current].TrimStart('-');

                // Parse optional value
                bool flagHasValue = next < args.Length && !args[next].StartsWith("-");
                string value = flagHasValue ? args[next].TrimStart('-') : "";
                bool secret = Secrets.Contains(flag);
                string displayValue = secret ? "*HIDDEN*" : "\"" + value + "\"";

                // Assign
                if (providedArguments.ContainsKey(flag))
                {
                    Console.WriteLine($"Flag \"{flag}\" is already defined. Overwriting value with {displayValue}.");
                    providedArguments[flag] = value;
                }
                else
                {
                    Console.WriteLine($"Found flag \"{flag}\" with value {displayValue}.");
                    providedArguments.Add(flag, value);
                }
            }
        }

        static void Build(MyBuildTarget buildTarget, int buildSubtarget, string filePath)
        {
            string[] scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(s => s.path).ToArray();
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                target = GetBuildTarget(buildTarget),
                locationPathName = filePath,
                //                options = UnityEditor.BuildOptions.Development
#if UNITY_2021_2_OR_NEWER
                subtarget = buildSubtarget
#endif
            };
            BuildTargetGroup buildTargetGroup = GetBuildTargetGroup(buildTarget);
            if (buildTargetGroup > 0)
                buildPlayerOptions.targetGroup = buildTargetGroup;

            BuildSummary buildSummary = BuildPipeline.BuildPlayer(buildPlayerOptions).summary;
            ReportSummary(buildSummary);
            ExitWithResult(buildSummary.result);
        }

        static void ReportSummary(BuildSummary summary)
        {
            Console.WriteLine(
                $"{Eol}" +
                $"###########################{Eol}" +
                $"#      Build results      #{Eol}" +
                $"###########################{Eol}" +
                $"{Eol}" +
                $"Duration: {summary.totalTime}{Eol}" +
                $"Warnings: {summary.totalWarnings}{Eol}" +
                $"Errors: {summary.totalErrors}{Eol}" +
                $"Size: {summary.totalSize} bytes{Eol}" +
                $"{Eol}"
            );
        }

        static void ExitWithResult(BuildResult result)
        {
            switch (result)
            {
                case BuildResult.Succeeded:
                    Console.WriteLine("Build succeeded!");
                    EditorApplication.Exit(0);
                    break;
                case BuildResult.Failed:
                    Console.WriteLine("Build failed!");
                    EditorApplication.Exit(101);
                    break;
                case BuildResult.Cancelled:
                    Console.WriteLine("Build cancelled!");
                    EditorApplication.Exit(102);
                    break;
                case BuildResult.Unknown:
                default:
                    Console.WriteLine("Build result is unknown!");
                    EditorApplication.Exit(103);
                    break;
            }
        }

        static NamedBuildTarget GetNamedBuildTarget(MyBuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case MyBuildTarget.StandaloneWindowsX86:
                case MyBuildTarget.StandaloneWindowsX86_64:
                case MyBuildTarget.StandaloneWindowsArm64:
                case MyBuildTarget.StandaloneLinuxX86_64:
                    return NamedBuildTarget.Standalone;
                case MyBuildTarget.Android:
                    return NamedBuildTarget.Android;
                default:
                    return NamedBuildTarget.Unknown;
            }
        }

        static BuildTarget GetBuildTarget(MyBuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case MyBuildTarget.StandaloneWindowsX86:
                    return BuildTarget.StandaloneWindows;
                case MyBuildTarget.StandaloneWindowsX86_64:
                    return BuildTarget.StandaloneWindows64;
                case MyBuildTarget.StandaloneWindowsArm64:
                    return BuildTarget.StandaloneWindows64;
                case MyBuildTarget.StandaloneLinuxX86_64:
                    return BuildTarget.StandaloneLinux64;
                case MyBuildTarget.Android:
                    return BuildTarget.Android;
                default:
                    return BuildTarget.NoTarget;
            }
        }

        static BuildTargetGroup GetBuildTargetGroup(MyBuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case MyBuildTarget.StandaloneWindowsX86:
                case MyBuildTarget.StandaloneWindowsX86_64:
                case MyBuildTarget.StandaloneWindowsArm64:
                case MyBuildTarget.StandaloneLinuxX86_64:
                    return BuildTargetGroup.Standalone;
                case MyBuildTarget.Android:
                    return BuildTargetGroup.Android;
                default:
                    return BuildTargetGroup.Unknown;
            }
        }
    }
}