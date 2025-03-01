using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace TFPlay.BuildTools
{
    public class FixBundleIdentifier : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            TryChangeBundleIdentifier();
        }

        private static void TryChangeBundleIdentifier()
        {
            if (PlayerSettings.applicationIdentifier == "com.game.name")
            {
                var path = Path.GetDirectoryName(Application.dataPath);
                var lastFolderName = Path.GetFileName(path);
                var newTempBundleIdentifier = "com.game." + RemoveNumbers(lastFolderName).ToLowerInvariant();
                PlayerSettings.SetApplicationIdentifier(EditorUserBuildSettings.selectedBuildTargetGroup, newTempBundleIdentifier);
                EditorUtility.DisplayDialog("ACHTUNG!!!", $"The Bundle Id was automatically changed to \"{newTempBundleIdentifier}\".\n\nPlease change it later.", "OK");
            }
        }

        private static string RemoveNumbers(string input)
        {
            string pattern = @"[\d-]";
            string replacement = string.Empty;
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(input, replacement);
            return result;
        }
    }
}
