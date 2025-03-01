using System;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace TFPlay.BuildTools
{
    public class DeleteBurstDirectories : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            DeleteBurstDebugInformationFolder(report, "BurstDebugInformation_DoNotShip");
            DeleteBurstDebugInformationFolder(report, "BackUpThisFolder_ButDontShipItWithYourGame");
        }

        private void DeleteBurstDebugInformationFolder(BuildReport buildReport, string directoryNameSufix)
        {
            string outputPath = buildReport.summary.outputPath;
            try
            {
                string applicationName = Path.GetFileNameWithoutExtension(outputPath);
                string outputFolder = Path.GetDirectoryName(outputPath);
                outputFolder = Path.GetFullPath(outputFolder);
                string burstDebugInformationDirectoryPath = Path.Combine(outputFolder, $"{applicationName}_{directoryNameSufix}");

                if (Directory.Exists(burstDebugInformationDirectoryPath))
                {
                    Debug.Log($" > Deleting Burst debug information folder at path '{burstDebugInformationDirectoryPath}'...");
                    Directory.Delete(burstDebugInformationDirectoryPath, true);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"An unexpected exception occurred while performing build cleanup: {e}");
            }
        }
    }
}
