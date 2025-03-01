using UnityEngine;
using UnityEditor;

namespace TFPlay.SafeArea
{
    public static class SafeAreaEnabler
    {
        private static SafeAreaVisualizer safeAreaVisualizer;

        public static void SetEnabled(bool enabled)
        {
            if (enabled)
            {
                if (safeAreaVisualizer == null)
                {
                    Show();
                }
            }
            else
            {
                if (safeAreaVisualizer != null)
                {
                    Hide();
                }
            }
        }

        private static bool TryFindComponent()
        {
            if (safeAreaVisualizer == null)
            {
                safeAreaVisualizer = MonoBehaviour.FindObjectOfType<SafeAreaVisualizer>();
            }
            return safeAreaVisualizer != null;
        }

        private static void Show()
        {
            if (!TryFindComponent())
            {
                var prefab = Resources.Load<GameObject>("SafeAreaVisualizer");
                if (prefab == null)
                {
                    Debug.LogError("SafeAreaVisualizer prefab not found in Resources.");
                    return;
                }

                var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                if (instance == null)
                {
                    Debug.LogError("Failed to instantiate SafeAreaVisualizer prefab.");
                    return;
                }

                safeAreaVisualizer = instance.GetComponent<SafeAreaVisualizer>();
                if (safeAreaVisualizer == null)
                {
                    Debug.LogError("SafeAreaVisualizer component is missing in the prefab.");
                    MonoBehaviour.DestroyImmediate(instance);
                    return;
                }

                safeAreaVisualizer.transform.SetSiblingIndex(int.MaxValue);
                safeAreaVisualizer.gameObject.hideFlags = HideFlags.DontSaveInBuild;
            }
        }

        private static void Hide()
        {
            if (safeAreaVisualizer != null)
            {
                MonoBehaviour.DestroyImmediate(safeAreaVisualizer.gameObject);
                safeAreaVisualizer = null;
            }
        }
    }
}
