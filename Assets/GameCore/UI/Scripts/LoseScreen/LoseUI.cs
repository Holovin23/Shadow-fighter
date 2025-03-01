using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TFPlay.Modules.Levels;

namespace NewUI
{
    public class LoseUI : ScreenUI
    {
        [Inject] private ILevelsService _levelsService;

        [SerializeField] private Button _backgroundButton;

        public override void Init()
        {
            base.Init();
            _backgroundButton.onClick.AddListener(HideLose);
        }

        private void HideLose()
        {
            Debug.Log("Lose closed");
            Hide();
            _levelsService.Restart();
        }
    }
}