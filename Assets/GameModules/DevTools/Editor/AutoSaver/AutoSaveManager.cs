using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TFPlay.AutoSaverTool
{
    [InitializeOnLoad]
    public static class AutoSaveManager
    {
        public static bool autoSaveEnabled;
        public static float saveInterval;
        private static float nextSaveTime;

        private const string AutoSaveEnabledKey = "AutoSaveEnabled";
        private const string SaveIntervalKey = "SaveInterval";

        static AutoSaveManager()
        {
            autoSaveEnabled = EditorPrefs.GetBool(AutoSaveEnabledKey, false);
            saveInterval = EditorPrefs.GetFloat(SaveIntervalKey, 300f);
            nextSaveTime = (float)EditorApplication.timeSinceStartup + saveInterval;

            EditorApplication.update += Update;
            // SceneView.duringSceneGui += OnSceneGUI;
            EditorApplication.delayCall += AddToolbarButton;
        }

        public static void SetAutoSaveEnabled(bool enabled)
        {
            autoSaveEnabled = enabled;
            EditorPrefs.SetBool(AutoSaveEnabledKey, autoSaveEnabled);
            nextSaveTime = (float)EditorApplication.timeSinceStartup + saveInterval;
        }

        public static void SetSaveInterval(float interval)
        {
            saveInterval = interval;
            EditorPrefs.SetFloat(SaveIntervalKey, saveInterval);
            nextSaveTime = (float)EditorApplication.timeSinceStartup + saveInterval;
        }

        private static void Update()
        {
            if (autoSaveEnabled && EditorApplication.timeSinceStartup >= nextSaveTime)
            {
                SaveScene();
                nextSaveTime = (float)EditorApplication.timeSinceStartup + saveInterval;
            }
        }

        public static void SaveScene()
        {
            var scenesDirty = false;
            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                var scene = EditorSceneManager.GetSceneAt(i);
                if (scene.isDirty)
                {
                    scenesDirty = true;
                    break;
                }
            }

            if (!scenesDirty) return;
            EditorSceneManager.SaveOpenScenes();
            Debug.Log("Scenes saved at: " + System.DateTime.Now);
        }

        private static void AddToolbarButton()
        {
            ToolbarCallback.OnToolbarGUILeft += OnToolbarGUI;
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            GUIStyle toggleStyle = new GUIStyle(GUI.skin.button);
            toggleStyle.normal.textColor = autoSaveEnabled ? Color.green : Color.red;

            if (GUILayout.Button(new GUIContent("Auto Save", "Toggle Auto Save"), toggleStyle))
            {
                SetAutoSaveEnabled(!autoSaveEnabled);
                var status = autoSaveEnabled ? "ON" : "OFF";
                Debug.Log("AutoSave " + status);
            }
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            Handles.BeginGUI();

            GUILayout.BeginArea(new Rect(10, 10, 100, 100));

            GUIStyle toggleStyle = new GUIStyle(GUI.skin.button);
            toggleStyle.normal.textColor = autoSaveEnabled ? Color.green : Color.red;

            if (GUILayout.Button(new GUIContent("Auto Save", "Toggle Auto Save"), toggleStyle))
            {
                SetAutoSaveEnabled(!autoSaveEnabled);
                var status = autoSaveEnabled ? "ON" : "OFF";
                Debug.Log("AutoSave " + status);
            }

            GUILayout.EndArea();

            Handles.EndGUI();
        }
    }
}