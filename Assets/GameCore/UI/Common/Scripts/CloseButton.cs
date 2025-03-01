using UnityEngine;
using UnityEngine.UI;

namespace TFPlay.UI
{
    public class CloseButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private BaseUI _closeUI;

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _closeUI.Hide();
        }
    }
}