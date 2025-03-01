using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace TFPlay.DeveloperUtilities
{
    public class PerfomanceTesterButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Button button;

        private Func<bool> predicate;
        private Action<bool> execute;

        private void OnValidate()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
            button = GetComponent<Button>();
        }

        public void Init(string buttonText, Func<bool> predicate, Action<bool> execute)
        {
            text.text = buttonText;
            this.execute = execute;
            this.predicate = predicate;
            button.onClick.AddListener(OnButtonClicked);
            ChangeColor(predicate.Invoke());
        }

        private void OnButtonClicked()
        {
            var enabled = !predicate.Invoke();
            execute.Invoke(enabled);
            ChangeColor(enabled);
        }

        private void ChangeColor(bool enabled)
        {
            button.image.color = enabled ? Color.green : Color.red;
        }
    }
}
