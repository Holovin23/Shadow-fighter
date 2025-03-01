using System.IO;
using UnityEngine;
using UnityEditor;

namespace TFPlay.EditorTools
{
    public abstract class ResourceSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;
        private const string AssetPath = "Assets/Settings";

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath<T>(GetAssetPath());
                    if (instance == null)
                    {
                        CreateAsset();
                    }
                    var inst = instance as ResourceSingleton<T>;
                    if (inst != null)
                    {
                        inst.OnInstanceLoaded();
                    }
                }
                return instance;
            }
        }

        public void Make()
        {

        }

        static void CreateAsset()
        {
            instance = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, GetAssetPath());
            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = instance;
        }

        public void Save()
        {
            if (instance != null)
            {
                EditorUtility.SetDirty(instance);
                AssetDatabase.SaveAssets();
            }
        }

        protected virtual void OnInstanceLoaded()
        {

        }

        private static string GetAssetName()
        {
            string assetName = typeof(T).Name + ".asset";
            return assetName;
        }

        private static string GetAssetPath()
        {
            string path = Path.Combine(AssetPath, GetAssetName());
            return path;
        }
    }
}