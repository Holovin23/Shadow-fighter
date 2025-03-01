using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TFPlay.DeveloperUtilities
{
    public class AccessPanel : MonoBehaviour
    {
        private const string PASSWORD = "2424";

        [SerializeField] private Transform content;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_InputField passwordInputField;

        public event System.Action OnAccessGranted;
        public event System.Action OnExit;

        private void Start()
        {
            loginButton.onClick.AddListener(OnLoginButtonClicked);
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnLoginButtonClicked()
        {
            if (passwordInputField.text.Equals(PASSWORD))
            {
                Hide();
                OnAccessGranted?.Invoke();
            }
        }

        private void OnCloseButtonClicked()
        {
            Hide();
            OnExit?.Invoke();
        }

        public void Show()
        {
            content.SetActive();
        }

        public void Hide()
        {
            content.SetInactive();
        }
    }
}
