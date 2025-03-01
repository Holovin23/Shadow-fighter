using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using TFPlay.Modules.Levels;

namespace NewUI
{
    public class WinUI : ScreenUI
    {
        [Inject] private ILevelsService _levelsService;

        [SerializeField] private Button _backgroundButton;
        [SerializeField] private StarsUI _starsUI;
        private int currentStarts = 0;

        public override void Init()
        {
            base.Init();
            _backgroundButton.onClick.AddListener(HideWin);
        }

        public void Show(int starsCount = 0)
        {
            currentStarts = starsCount;
            base.Show();
        }

        public override void Show()
        {
            this.Show(0);
        }

        protected override Tween StartUnfadeAnimation()
        {
            var result = DOTween.Sequence();
            result.Append(base.StartUnfadeAnimation());
            result.Append(_starsUI.StartAnimation(currentStarts));
            return result;
        }

        private void HideWin()
        {
            Hide();
            _levelsService.Next();
        }
    }
}