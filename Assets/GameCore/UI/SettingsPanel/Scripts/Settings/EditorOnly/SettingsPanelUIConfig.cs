using UnityEditor;
using UnityEngine;

namespace TFPlay.UI.SettingsUI
{
    public class SettingsPanelUIConfig : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Toggles")]
        [SerializeField] private bool _music;
        [SerializeField] private bool _sound;
        [SerializeField] private bool _vibration;
        [SerializeField] private bool _graphics;
        [SerializeField] private bool _restorePurchases;

        [Header("UI element References")]
        [SerializeField] private GameObject _musicUI;
        [SerializeField] private GameObject _soundUI;
        [SerializeField] private GameObject _vibrationUI;
        [SerializeField] private GameObject _graphicUI;
        [SerializeField] private GameObject _restorePurchasesUI;

        [Header("Other References")]
        [SerializeField] private SettingsWindowResizer _settingsWindowResizer;

        private bool _changed;

        private void OnValidate()
        {
            Check(_musicUI, _music);
            Check(_soundUI, _sound);
            Check(_vibrationUI, _vibration);
            Check(_graphicUI, _graphics);
            Check(_restorePurchasesUI, _restorePurchases);

            Changed();
        }

        private void Check(GameObject gameObject, bool enabled)
        {
            if (gameObject.activeSelf == enabled)
                return;

            gameObject.SetActive(enabled);
            _changed = true;
        }

        private void Changed()
        {
            if (!_changed)
                return;

            _settingsWindowResizer.Resize();
            EditorUtility.SetDirty(gameObject);
            _changed = false;
        }
#endif
    }
}