using UnityEngine;
using UnityEngine.UI;

namespace TFPlay.UI
{
    public class OpenButton : BaseButton
    {
        [SerializeField] private Button _button;
        [SerializeField] private BaseUI _ui;

        public override void Initialize()
        {
            _button.onClick.AddListener(() => _ui.Show());
        }
    }
}