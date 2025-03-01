using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

namespace TFPlay.Features.RatePopup
{
    public class CustomRatePopupUI : BaseRatePopupCustomView
    {
        [System.Serializable]
        public class Star
        {
            public Image icon;
            public Animator animator;
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Transform mainContent;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button[] starButtons;

        [SerializeField] private Sprite fullStar;
        [SerializeField] private Sprite emptyStar;
        [SerializeField] private Star[] stars;

        private int rating = 0;
        private IRatePopupProvider ratingPopupProvider;


        public override void Initialize(IRatePopupProvider ratingPopupProvider)
        {
            this.ratingPopupProvider = ratingPopupProvider;

            submitButton.onClick.AddListener(OnSumbitButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);

            for (int i = 0; i < starButtons.Length; i++)
            {
                var index = i; //fix closure
                starButtons[i].onClick.AddListener(() => OnStarButtonClicked(index));
            }

            HideInstant();
        }

        public override void Show()
        {
            UpdateStarsVisual(5);

            canvasGroup.DOFade(1f, 0.25f).From(0f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
            mainContent.transform.DOScale(1f, 0.25f).From(0f).SetEase(Ease.OutBack);
        }

        public override void Hide()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0f, 0.25f).SetEase(Ease.InBack).From(1f).SetLink(gameObject).OnComplete(() => Destroy(gameObject));
            mainContent.transform.DOScale(0f, 0.25f).From(1f).SetEase(Ease.InBack).SetLink(gameObject);
        }

        private void Start()
        {
            StartCoroutine(ShowDelayed(1));
        }

        private void OnStarButtonClicked(int index)
        {
            UpdateStarsVisual(index);
            rating = index + 1;
        }

        private void OnCancelButtonClicked()
        {
            Hide();
        }

        private void OnSumbitButtonClicked()
        {
            if (rating > 4)
            {
                ratingPopupProvider.ShowRatePopup();
            }

            Hide();
        }

        private void UpdateStarsVisual(int index)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                bool isActive = i <= index;
                stars[i].icon.sprite = isActive ? fullStar : emptyStar;
                stars[i].animator.SetBool("Active", isActive);

                if (isActive)
                {
                    stars[i].animator.Play("Shine", 0, 1 - (float)i / stars.Length);
                }
            }
        }

        private IEnumerator ShowDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);
            Show();
        }

        private void HideInstant()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
            mainContent.transform.localScale = Vector3.zero;
        }
    }
}

