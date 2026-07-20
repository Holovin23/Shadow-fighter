using UnityEngine;
using System.IO;

namespace SimpleSave.Settings
{
    [CreateAssetMenu(fileName = "SaveSettings", menuName = "SimpleSave/Settings")]
    public class SimpleSaveSettings : ScriptableObject
    {
        public enum SaveFormat { Binary, Json, PlayerPrefs }

        public SaveFormat saveFormat = SaveFormat.Binary;
        public string encryptionPassword = "";
        public bool useEncryption = false;

        public static SimpleSaveSettings Instance {
            get {
                if (_instance == null)
                {
#if UNITY_EDITOR
                    string assetPath = "Assets/Resources/SimpleSave/Settings/SaveSettings.asset";
                    _instance = UnityEditor.AssetDatabase.LoadAssetAtPath<SimpleSaveSettings>(assetPath);
                    if (_instance == null)
                    {
                        _instance = ScriptableObject.CreateInstance<SimpleSaveSettings>();
                        Directory.CreateDirectory("Assets/Resources/SimpleSave/Settings");
                        UnityEditor.AssetDatabase.CreateAsset(_instance, assetPath);
                        UnityEditor.AssetDatabase.SaveAssets();
                        Debug.Log("[SimpleSave] Auto-created SaveSettings.asset at " + assetPath);
                    }
#else
                    _instance = Resources.Load<SimpleSaveSettings>("SimpleSave/Settings/SaveSettings");
#endif
                }
                return _instance;
            }
        }
        private static SimpleSaveSettings _instance;

        public string GetPath(string key) {
            return Path.Combine(Application.persistentDataPath, key + GetExtension());
        }

        public string GetExtension() {
            return saveFormat switch {
                SaveFormat.Json => ".json",
                SaveFormat.PlayerPrefs => "",
                _ => ".bin"
            };
        }
    }
}