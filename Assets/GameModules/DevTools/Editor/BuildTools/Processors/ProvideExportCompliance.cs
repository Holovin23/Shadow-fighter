#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Xcode = UnityEditor.iOS.Xcode;

namespace TFPlay.BuildTools
{
    public class ProvideExportCompliance : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.iOS)
            {
                var plistPath = report.summary.outputPath + "/Info.plist";
                var plist = new Xcode.PlistDocument();
                plist.ReadFromFile(plistPath);
                var rootDict = plist.root;
                rootDict.values.Add("ITSAppUsesNonExemptEncryption", new Xcode.PlistElementBoolean(false));
                File.WriteAllText(plistPath, plist.WriteToString());
            }
        }
    }
}
#endif
