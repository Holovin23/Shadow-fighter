using UnityEditor;
using UnityEngine;
using System.IO;

namespace SimpleSave
{
    public class SimpleSaveManager : EditorWindow
    {
        private Vector2 scroll;
        private string[] files;
        private SerializedObject serializedSettings;
        private int tab;

        [MenuItem("Tools/SimpleSaveManager")]
        public static void OpenWindow()
        {
            GetWindow<SimpleSaveManager>("SimpleSave Manager");
        }

        void OnEnable()
        {
            Refresh();
            var settings = SimpleSave.Settings.SimpleSaveSettings.Instance;
            serializedSettings = new SerializedObject(settings);
        }

        void Refresh()
        {
            files = Directory.GetFiles(Application.persistentDataPath);
        }

        void OnGUI()
        {
            tab = GUILayout.Toolbar(tab, new[] { "Settings", "Saves" });

            if (tab == 0)
            {
                DrawSettingsTab();
            }
            else
            {
                DrawSavesTab();
            }
        }

        void DrawSavesTab()
        {
            if (GUILayout.Button("🔄 Refresh")) Refresh();

            if (GUILayout.Button("🗑️ Delete All"))
            {
                foreach (var f in files)
                {
                    try
                    {
                        File.Delete(f);
                    }
                    catch
                    {
                    }
                }

                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Refresh();
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var f in files)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(Path.GetFileName(f));

                if (GUILayout.Button("Open", GUILayout.Width(60)))
                    EditorUtility.OpenWithDefaultApp(f);
                if (GUILayout.Button("Show in Explorer", GUILayout.Width(120)))
                    EditorUtility.RevealInFinder(f);
                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    File.Delete(f);
                    Refresh();
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        void DrawSettingsTab()
        {
            if (serializedSettings == null) return;

            serializedSettings.Update();

            EditorGUILayout.LabelField("Save Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("saveFormat"));
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("useEncryption"));
            EditorGUILayout.PropertyField(serializedSettings.FindProperty("encryptionPassword"));

            serializedSettings.ApplyModifiedProperties();
        }
    }
}