using UnityEditor;
using UnityEngine;

namespace TFPlay.AutoSaverTool
{
    public class AutoSaveToolEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Auto Save Settings")]
        public static void ShowWindow()
        {
            GetWindow<AutoSaveToolEditorWindow>("Auto Save Settings");
        }

        private void OnEnable()
        {
            AutoSaveManager.SetAutoSaveEnabled(EditorPrefs.GetBool("AutoSaveEnabled", false));
            AutoSaveManager.SetSaveInterval(EditorPrefs.GetFloat("SaveInterval", 300f));
        }

        private void OnDisable()
        {
            EditorPrefs.SetBool("AutoSaveEnabled", AutoSaveManager.autoSaveEnabled);
            EditorPrefs.SetFloat("SaveInterval", AutoSaveManager.saveInterval);
        }

        private void OnGUI()
        {
            GUILayout.Label("Auto Save Settings", EditorStyles.boldLabel);

            bool autoSaveEnabled = EditorGUILayout.Toggle("Enable Auto Save", AutoSaveManager.autoSaveEnabled);
            if (autoSaveEnabled != AutoSaveManager.autoSaveEnabled)
            {
                AutoSaveManager.SetAutoSaveEnabled(autoSaveEnabled);
            }

            float saveInterval = EditorGUILayout.FloatField("Save Interval (seconds)", AutoSaveManager.saveInterval);
            if (saveInterval != AutoSaveManager.saveInterval)
            {
                AutoSaveManager.SetSaveInterval(saveInterval);
            }

            if (GUILayout.Button("Save Now"))
            {
                AutoSaveManager.SaveScene();
            }
        }
    }
}