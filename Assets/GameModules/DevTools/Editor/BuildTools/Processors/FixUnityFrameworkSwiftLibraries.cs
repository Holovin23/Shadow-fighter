#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Xcode = UnityEditor.iOS.Xcode;

namespace TFPlay.BuildTools
{
    public class FixUnityFrameworkSwiftLibraries : IPostprocessBuildWithReport
    {
        public int callbackOrder => 999;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.iOS)
            {
                var projPath = report.summary.outputPath + "/Unity-iPhone.xcodeproj/project.pbxproj";

                var project = new Xcode.PBXProject();
                project.ReadFromString(File.ReadAllText(projPath));

                var mainTargetGuid = project.GetUnityMainTargetGuid();
                project.SetBuildProperty(mainTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

                var unityFrameWorkTargetGuid = project.GetUnityFrameworkTargetGuid();
                project.SetBuildProperty(unityFrameWorkTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
                project.AddBuildProperty(unityFrameWorkTargetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/../../Frameworks");

                File.WriteAllText(projPath, project.WriteToString());
            }
        }
    }
}
#endif