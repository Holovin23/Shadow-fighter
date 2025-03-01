#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Xcode = UnityEditor.iOS.Xcode;

namespace TFPlay.BuildTools
{
    public class DisableBitcode : IPostprocessBuildWithReport
    {
        public int callbackOrder => 999;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.iOS)
            {
                var projPath = report.summary.outputPath + "/Unity-iPhone.xcodeproj/project.pbxproj";

                var project = new Xcode.PBXProject();
                project.ReadFromFile(projPath);

                var mainTargetGuid = project.GetUnityMainTargetGuid();
                project.SetBuildProperty(mainTargetGuid, "ENABLE_BITCODE", "NO");

                var testTargetGuid = project.TargetGuidByName(Xcode.PBXProject.GetUnityTestTargetName());
                project.SetBuildProperty(testTargetGuid, "ENABLE_BITCODE", "NO");

                var unityFrameWorkTargetGuid = project.GetUnityFrameworkTargetGuid();
                project.SetBuildProperty(unityFrameWorkTargetGuid, "ENABLE_BITCODE", "NO");

                project.WriteToFile(projPath);
            }
        }
    }
}
#endif