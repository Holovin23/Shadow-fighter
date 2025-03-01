using UnityEngine;
using DG.Tweening;
using Zenject;
using NewUI.Animations;
using TFPlay.UI;

namespace NewUI
{
    public abstract class ScreenUI : MonoBehaviour
    {
        [SerializeField] private ScreenID _id;
        [SerializeField] private bool _hideOnRegister = false;
        [Space(10)]
        [Header("Animations")]
        [SerializeField] private FadeAnimationSettings _unfadeAnimation;
        [SerializeField] private FadeAnimationSettings _fadeAnimation;
        [SerializeField] private bool _interactableOnFade = true;

        [SerializeField] private CanvasGroup _canvasGroup;

        [Inject] private ScreensService _screenService;

        private UIState _state = UIState.Hidden;
        private Tween _currentAnimation;

        public ScreenID ID { get { return _id; } }

        private void OnDisable()
        {
            _state = UIState.Hidden;
        }
        private void OnEnable()
        {
            _state = UIState.Shown;
        }

        public virtual void Init()
        {
            Register();

            InitStartState();
        }

        public virtual void Show()
        {
            if (_state == UIState.Shown)
                return;
            
            _state = UIState.Shown;

            _currentAnimation?.Kill();
            _currentAnimation = StartUnfadeAnimation().OnStart(OnStartUnfade).OnComplete(OnEndUnfade);
        }
        public virtual void Hide()
        {
            if (_state == UIState.Hidden)
                return;
            _state = UIState.Hidden;

            _currentAnimation?.Kill();
            _currentAnimation = StartFadeAnimation().OnStart(OnStartFade).OnComplete(OnEndFade);
        }
        protected virtual Tween StartUnfadeAnimation()
        {
            return _unfadeAnimation.DoFade(_canvasGroup);
        }
        protected virtual Tween StartFadeAnimation()
        {
            return _fadeAnimation.DoFade(_canvasGroup);
        }
        protected void Register()
        {
            _screenService.Register(this);
        }
        protected void InitStartState()
        {
            if (_hideOnRegister)
            {
                gameObject.SetInactive();
                _canvasGroup.alpha = 0;
            }
            else
            {
                gameObject.SetActive();
                _canvasGroup.alpha = 1;
            }
        }
        private void OnStartFade()
        {
            _canvasGroup.interactable = _interactableOnFade;
        }
        private void OnEndFade()
        {
            gameObject.SetInactive();
        }
        private void OnStartUnfade()
        {
            gameObject.SetActive();
            _canvasGroup.interactable = _interactableOnFade;
        }
        private void OnEndUnfade()
        {
            _canvasGroup.interactable = true;
        }
    }
}