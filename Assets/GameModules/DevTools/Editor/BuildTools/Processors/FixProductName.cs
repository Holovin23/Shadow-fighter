using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace TFPlay.BuildTools
{
    public class FixPRoductName : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            TryChangeProductName();
        }

        private static void TryChangeProductName()
        {
            if (PlayerSettings.productName == "MyGame")
            {
                var path = Path.GetDirectoryName(Application.dataPath);
                var lastFolderName = Path.GetFileName(path);
                var pattern = "[^A-Za-z0-9 ]";
                var newTempProductName = Regex.Replace(lastFolderName, pattern, " ");
                PlayerSettings.productName = newTempProductName;
                EditorUtility.DisplayDialog("ACHTUNG!!!", $"The Product Name was automatically changed to \"{newTempProductName}\".\n\nPlease change it later.", "OK");
            }
        }
    }
}
