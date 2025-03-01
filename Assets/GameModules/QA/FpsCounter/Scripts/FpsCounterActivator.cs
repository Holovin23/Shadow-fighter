using UnityEngine;

namespace TFPlay.DeveloperUtilities.FpsCounter
{
    public class FpsCounterActivator : MonoBehaviour
    {
        [SerializeField] private FpsCounter fpsCounter;
        [SerializeField] private int clicksNeeded = 10;
        [SerializeField] private int buttonSize = 64;

        private int _clicks;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            var fpsCounterActivatorPrefab = Resources.Load<FpsCounterActivator>(nameof(FpsCounterActivator));
            var fpsCounterActivator = GameObject.Instantiate(fpsCounterActivatorPrefab);
            fpsCounterActivator.name = "[FpsCounterActivator]";
            GameObject.DontDestroyOnLoad(fpsCounterActivator);
        }

        private void OnGUI()
        {
            DrawInvisibleButton();
        }

        private void DrawInvisibleButton()
        {
            var buttonWidth = buttonSize * GetAspectRatioCoefficient();
            var buttonHeight = buttonSize * GetAspectRatioCoefficient();
            var buttonPositionX = Screen.width - buttonWidth;
            var buttonPositionY = 0f;

            if (GUI.Button(new Rect(buttonPositionX, buttonPositionY, buttonWidth, buttonHeight), "", GetinvisibleStyle()))
            {
                _clicks++;
                if (_clicks >= clicksNeeded)
                {
                    _clicks = 0;
                    fpsCounter.gameObject.SetActive(!fpsCounter.gameObject.activeSelf);
                }
            }
        }

        private float GetAspectRatioCoefficient()
        {
            return (float)Screen.height / 1920;
        }

        private GUIStyle GetinvisibleStyle()
        {
            var invisibleStyle = new GUIStyle(GUI.skin.label);
            invisibleStyle.normal.background = null;
            invisibleStyle.hover.background = null;
            invisibleStyle.active.background = null;
            return invisibleStyle;
        }
    }
}
