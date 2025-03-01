using UnityEngine;
using Zenject;

namespace TFPlay.Modules.UI.PopUpService
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private PopUpType popUpType;
        [SerializeField] private GameObject content;

        [Inject] private PopUpService _popUpService;

        private void Start()
        {
            _popUpService.Register(this, popUpType);
        }

        public void Show()
        {
            content.SetActive();
        }

        public void Hide()
        {
            content.SetInactive();
            _popUpService.PopupClosed();
        }
    }
}