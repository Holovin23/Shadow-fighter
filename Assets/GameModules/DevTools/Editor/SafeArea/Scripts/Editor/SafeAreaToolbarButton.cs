using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityToolbarExtender;

namespace TFPlay.SafeArea
{
    [InitializeOnLoad]
    public static class SafeAreaToolbarButton
    {
        private const string ENABLED_TEXT = "Show safe area";
        private const string DISABLED_TEXT = "Hide safe area";
        private const string SAFE_AREA_VISUALIZE_ENABLED_KEY = "SafeAreaVisualizeEnabled";
        private const string SAFE_AREA_ICON_NAME = @"UnityEditor.Timeline.TimelineWindow@2x";

        private static bool safeAreaEnabled;

        static SafeAreaToolbarButton()
        {
            EditorApplication.update += Init;
        }

        static void Init()
        {
            EditorApplication.update -= Init;
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            safeAreaEnabled = EditorPrefs.GetBool(SAFE_AREA_VISUALIZE_ENABLED_KEY, false);

            var image = EditorGUIUtility.IconContent(SAFE_AREA_ICON_NAME).image;
            var content = new GUIContent(null, image, safeAreaEnabled ? DISABLED_TEXT : ENABLED_TEXT);
            safeAreaEnabled = GUILayout.Toggle(safeAreaEnabled, content, "Command");

            SafeAreaEnabler.SetEnabled(safeAreaEnabled);
            EditorPrefs.SetBool(SAFE_AREA_VISUALIZE_ENABLED_KEY, safeAreaEnabled);
        }

        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            safeAreaEnabled = EditorPrefs.GetBool(SAFE_AREA_VISUALIZE_ENABLED_KEY, false);
        }
    }
}
