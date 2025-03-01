using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace TFPlay.BuildTools
{
    public class AutoBuilder
    {
        private const string BuildHelperMenuBasePath = "Build Helper/";
        private const string AutoBuilderMenuPath = BuildHelperMenuBasePath + "Auto Builder/";
        private const string BuildAndroidRegularMenuPath = AutoBuilderMenuPath + "Build Android Regular";
        private const string BuildAndroidReleaseMenuPath = AutoBuilderMenuPath + "Build Android Release";
        private const string BuildiOSReleaseMenuPath = AutoBuilderMenuPath + "Build iOS Release";

        private static readonly string GameName = PlayerSettings.productName.Replace(" ", "");
        private static readonly string APP_FOLDER = Directory.GetCurrentDirectory();
        private static readonly string ANDROID_FOLDER = string.Format("{0}/Builds/android/", APP_FOLDER);
        private static readonly string IOS_FOLDER = string.Format("{0}/Builds/ios/unity/", APP_FOLDER);

        private const string CMD_LINE_ARG_DEVELOPMENT_BUILD = "developmentBuild";

        private const string CMD_LINE_ARG_BUILD_VERSION = "buildVersion";
        private const string CMD_LINE_ARG_ANDROID_BUILD_NUMBER = "androidBuildNumber";
        private const string CMD_LINE_ARG_IOS_BUILD_NUMBER = "iOSBuildNumber";
        private const string CMD_LINE_ARG_JOB_BUILD_NUMBER = "jobBuildNumber";

        private const string CMD_LINE_ARG_KEYSTORE_PATH = "keystorePath";
        private const string CMD_LINE_ARG_KEYSTORE_PASS = "keystorePass";
        private const string CMD_LINE_ARG_KEYSTORE_ALIAS = "keystoreAlias";
        private const string CMD_LINE_ARG_KEYSTORE_ALIAS_PASS = "keystoreAliasPass";

        private const string CMD_LINE_ARG_GRADLE_LOCATION = "gradleLocation";
        private const string CMD_LINE_ARG_GRADLE_VERSION = "gradleVersion";

        private static readonly List<string> ValidCommandLineArguments = new List<string>()
        {
            CMD_LINE_ARG_DEVELOPMENT_BUILD,
            CMD_LINE_ARG_BUILD_VERSION,
            CMD_LINE_ARG_ANDROID_BUILD_NUMBER,
            CMD_LINE_ARG_IOS_BUILD_NUMBER,
            CMD_LINE_ARG_JOB_BUILD_NUMBER,
            CMD_LINE_ARG_KEYSTORE_PATH,
            CMD_LINE_ARG_KEYSTORE_PASS,
            CMD_LINE_ARG_KEYSTORE_ALIAS,
            CMD_LINE_ARG_KEYSTORE_ALIAS_PASS,
            CMD_LINE_ARG_GRADLE_LOCATION,
            CMD_LINE_ARG_GRADLE_VERSION
        };

        private static string BuildLocation;
        private static Dictionary<string, string> CommandLineArgs;

        [MenuItem(BuildAndroidRegularMenuPath)]
        public static void BuildAndroid_Regular()
        {
            ProcessCommandLineArgs();
            ProcessBuildVersion();
            ProcessAndroidBuildNumber();
            SetGradleVersion();
            BuildAndroid(IsDevelopmentBuild(), false);
        }

        [MenuItem(BuildAndroidReleaseMenuPath)]
        public static void BuildAndroid_Release()
        {
            ProcessCommandLineArgs();
            ProcessBuildVersion();
            ProcessAndroidBuildNumber();
            SetGradleVersion();
            BuildAndroid(false, true);
        }

        [MenuItem(BuildiOSReleaseMenuPath)]
        public static void BuildiOS_Release()
        {
            ProcessCommandLineArgs();
            ProcessBuildVersion();
            ProcessiOSBuildNumber();
            BuildiOS(false);
        }

        private static void BuildAndroid(bool developmentBuild, bool buildAppBundle)
        {
            BuildLocation = CreateOrGetBuildDirectory(BuildTarget.Android);
            Directory.CreateDirectory(BuildLocation);

            EditorUserBuildSettings.buildAppBundle = buildAppBundle;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            PlayerSettings.Android.useCustomKeystore = buildAppBundle;
            if (buildAppBundle)
            {
                ProcessKeystore();
            }
            PlayerSettings.Android.useAPKExpansionFiles = buildAppBundle;
            PlayerSettings.SplashScreen.showUnityLogo = false;

            var buildOptions = GetBuildOptions(developmentBuild);
            var locationPathName = Path.Combine(BuildLocation, GetBuildName(buildAppBundle, BuildTarget.Android));
            var playerOptions = CreatePlayerOptions(locationPathName, BuildTarget.Android, buildOptions);
            var report = BuildPipeline.BuildPlayer(playerOptions);

            if (report.summary.result == BuildResult.Failed)
            {
                if (Application.isBatchMode)
                {
                    EditorApplication.Exit(1);
                }
            }
        }

        private static void BuildiOS(bool developmentBuild)
        {
            BuildLocation = CreateOrGetBuildDirectory(BuildTarget.iOS);
            Directory.CreateDirectory(BuildLocation);

            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            PlayerSettings.SplashScreen.showUnityLogo = false;

            var buildOptions = GetBuildOptions(developmentBuild);
            var locationPathName = Path.Combine(BuildLocation, GetBuildName(BuildTarget.iOS));
            var buildPlayerOptions = CreatePlayerOptions(locationPathName, BuildTarget.iOS, buildOptions);
            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

            if (report.summary.result == BuildResult.Failed)
            {
                if (Application.isBatchMode)
                {
                    EditorApplication.Exit(1);
                }
            }
        }

        public static bool IsDevelopmentBuild()
        {
            var developmentBuild = false;
            if (CommandLineArgs.TryGetValue(CMD_LINE_ARG_DEVELOPMENT_BUILD, out string developmentBuildStr))
            {
                developmentBuild = bool.Parse(developmentBuildStr);
            }
            return developmentBuild;
        }

        private static void ProcessKeystore()
        {
            if (CommandLineArgs.TryGetValue(CMD_LINE_ARG_KEYSTORE_PATH, out string keyStorePath))
            {
                PlayerSettings.Android.keystoreName = keyStorePath;
            }
            if (CommandLineArgs.TryGetValue(CMD_LINE_ARG_KEYSTORE_PASS, out string keyStorePass))
            {
                PlayerSettings.Android.keystorePass = keyStorePass;
            }
            if (CommandLineArgs.TryGetValue(CMD_LINE_ARG_KEYSTORE_ALIAS, out string keyAliasName))
            {
                PlayerSettings.Android.keyaliasName = keyAliasName;
            }
            if (CommandLineArgs.TryGetValue(CMD_LINE_ARG_KEYSTORE_ALIAS_PASS, out string keyAliasPass))
            {
                PlayerSettings.Android.keyaliasPass = keyAliasPass;
            }
        }

        private static void ProcessBuildVersion()
        {
            if (CommandLineArgs.TryGetValue(CMD_LINE_ARG_BUILD_VERSION, out string buildVersion))
            {
                if (buildVersion == "0")
                {
                    return;
                }

                PlayerSettings.bundleVersion = buildVersion;
            }
        }

        private static void ProcessAndroidBuildNumber()
        {
            if (CommandLineArgs.TryGetValue(CMD_LINE_ARG_ANDROID_BUILD_NUMBER, out string androidBuildNumber))
            {
                var bundleVersionCode = int.Parse(androidBuildNumber);
                if (bundleVersionCode == 0)
                {
                    PlayerSettings.Android.bundleVersionCode = GetJobBuildNumber();
                }
                else
                {
                    PlayerSettings.Android.bundleVersionCode = bundleVersionCode;
                }
            }
        }

        private static void ProcessiOSBuildNumber()
        {
            if (CommandLineArgs.TryGetValue(CMD_LINE_ARG_IOS_BUILD_NUMBER, out string iosBuildNumber))
            {
                var buildNumber = int.Parse(iosBuildNumber);
                if (buildNumber == 0)
                {
                    PlayerSettings.iOS.buildNumber = GetJobBuildNumber().ToString();
                }
                else
                {
                    PlayerSettings.iOS.buildNumber = iosBuildNumber;
                }
            }
        }

        private static int GetJobBuildNumber()
        {
            if (CommandLineArgs.TryGetValue(CMD_LINE_ARG_JOB_BUILD_NUMBER, out string jobBuildNumber))
            {
                return int.Parse(jobBuildNumber);
            }
            return 0;
        }

        private static void ProcessCommandLineArgs()
        {
            CommandLineArgs = new Dictionary<string, string>();
            var args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args.Length > i + 1)
                {
                    if (ValidCommandLineArguments.Contains(args[i]))
                    {
                        CommandLineArgs.Add(args[i], args[i + 1]);
                    }
                }
            }
        }

        private static void SetGradleVersion()
        {
            if (CommandLineArgs.TryGetValue(CMD_LINE_ARG_GRADLE_VERSION, out string gradleVersion) && CommandLineArgs.TryGetValue(CMD_LINE_ARG_GRADLE_LOCATION, out string gradleLocation))
            {
                EditorPrefs.SetBool("GradleUseEmbedded", false);
                var gradlePath = gradleLocation + "/gradle-" + gradleVersion;
                EditorPrefs.SetString("GradlePath", gradlePath);
            }
            else
            {
                EditorPrefs.SetBool("GradleUseEmbedded", true);
            }
        }

        private static string CreateOrGetBuildDirectory(BuildTarget buildTarget)
        {
            var buildsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Builds");
            if (buildTarget == BuildTarget.Android)
            {
                buildsFolder = ANDROID_FOLDER;
            }
            else if (buildTarget == BuildTarget.iOS)
            {
                buildsFolder = IOS_FOLDER;
            }
            if (!Directory.Exists(buildsFolder))
            {
                Directory.CreateDirectory(buildsFolder);
            }
            return buildsFolder;
        }

        private static string GetBuildName(BuildTarget buildTarget)
        {
            return GetBuildName(false, buildTarget);
        }

        private static string GetBuildName(bool buildAppBundle, BuildTarget buildTarget)
        {
            var buildName = ReplaceInvalidChars(GameName);
            buildName += "-" + PlayerSettings.bundleVersion;
            if (buildTarget == BuildTarget.Android)
            {
                buildName += "(" + PlayerSettings.Android.bundleVersionCode + ")";
            }
            else if (buildTarget == BuildTarget.iOS)
            {
                buildName += "(" + PlayerSettings.iOS.buildNumber + ")";
            }
            if (buildTarget == BuildTarget.Android)
            {
                buildName += buildAppBundle ? ".aab" : ".apk";
            }
            return buildName;
        }

        private static BuildPlayerOptions CreatePlayerOptions(string locationPathName, BuildTarget platformName, BuildOptions buildOptions)
        {
            var buildPlayerOptions = new BuildPlayerOptions()
            {
                scenes = GetScenes(),
                locationPathName = locationPathName,
                target = platformName,
                options = buildOptions
            };
            return buildPlayerOptions;
        }

        private static BuildOptions GetBuildOptions(bool developmentBuild)
        {
            var buildOptions = BuildOptions.None;
            if (developmentBuild)
            {
                buildOptions |= BuildOptions.Development | BuildOptions.ConnectWithProfiler;
            }
            buildOptions |= BuildOptions.CompressWithLz4;
            return buildOptions;
        }

        private static string[] GetScenes()
        {
            var scenes = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                {
                    scenes.Add(EditorBuildSettings.scenes[i].path);
                }
            }
            return scenes.ToArray();
        }

        private static string ReplaceInvalidChars(string filename)
        {
            return string.Join("-", filename.Split(Path.GetInvalidFileNameChars())).Replace(":", "-");
        }
    }
}
