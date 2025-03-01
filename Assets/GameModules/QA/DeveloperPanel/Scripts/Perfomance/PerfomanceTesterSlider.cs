using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TFPlay.DeveloperUtilities
{
    public class PerfomanceTesterSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private Slider _slider;

        private string _baseHeaderText;
        private string _outputFormat;
        private Action<float> _execute;

        public void Init(string headerText, string outputFormat, float initialValue, Action<float> execute)
        {
            _baseHeaderText = headerText;
            _outputFormat = outputFormat;
            _execute = execute;
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
            UpdateValueText(initialValue);
        }

        private void OnSliderValueChanged(float value)
        {
            _execute.Invoke(value);
            UpdateValueText(value);
        }

        private void UpdateValueText(float value)
        {
            _header.text = _baseHeaderText + string.Format(_outputFormat, value);
        }
    }
}
