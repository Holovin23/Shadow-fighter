using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TFPlay.UI.SettingsUI
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Toggle _toggle;

        [SerializeField] private SettingMode _mode;

        public Action<float> OnChangeValue;
        public float Value { private set; get; }

        private bool _lockValue;

        public void Initialize(bool value)
        {
            Initialize(value ? 1 : 0);
        }

        public void Initialize(float value)
        {
            SetValue(value);

            _slider.onValueChanged.AddListener(OnSliderValueChanged);
            _toggle.onValueChanged.AddListener(OnToggleValueChange);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _slider.gameObject.SetActive(_mode is SettingMode.Slider or SettingMode.Both);
            _toggle.gameObject.SetActive(_mode is SettingMode.Toggle or SettingMode.Both);
            EditorUtility.SetDirty(this);
        }
#endif

        private void OnSliderValueChanged(float value)
        {
            SetValue(value);
        }

        private void OnToggleValueChange(bool value)
        {
            SetValue(value ? 1 : 0);
        }

        private void SetValue(float value)
        {
            if (_lockValue)
                return;

            _lockValue = true;

            Value = value;

            _slider.value = value;
            _toggle.isOn = value > 0;

            _lockValue = false;

            OnChangeValue?.Invoke(Value);
        }
    }
}