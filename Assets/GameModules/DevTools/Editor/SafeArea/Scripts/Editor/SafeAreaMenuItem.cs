using UnityEditor;

namespace TFPlay.SafeArea
{
    [InitializeOnLoad]
    public class SafeAreaMenuItem
    {
        private const string SAFE_AREA_TOOS_BUTTON = "Tools/Safe Area";
        private const string SAFE_AREA_VISUALIZE_ENABLED_KEY = "SafeAreaVisualizeEnabled";

        [MenuItem(SAFE_AREA_TOOS_BUTTON)]
        private static void ToggleFeature()
        {
            bool isEnabled = !EditorPrefs.GetBool(SAFE_AREA_VISUALIZE_ENABLED_KEY, false);
            EditorPrefs.SetBool(SAFE_AREA_VISUALIZE_ENABLED_KEY, isEnabled);

            SafeAreaEnabler.SetEnabled(isEnabled);
        }

        [MenuItem(SAFE_AREA_TOOS_BUTTON, true)]
        private static bool ToggleFeatureValidate()
        {
            Menu.SetChecked(SAFE_AREA_TOOS_BUTTON, EditorPrefs.GetBool(SAFE_AREA_VISUALIZE_ENABLED_KEY, false));
            return true;
        }
    }
}