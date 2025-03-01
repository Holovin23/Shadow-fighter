using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace TFPlay.DeveloperUtilities
{
    public class DeveloperPanelEnabler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private const float _timeToOpen = 5;

         [Inject] private DeveloperPanel _developerPanel;

        private Coroutine _waitAndShowCoroutine;

        public void OnPointerDown(PointerEventData eventData)
        {
            // if (_developerPanel != null && _settingsUI != null && _settingsUI.IsOpened)
            {
                _waitAndShowCoroutine = StartCoroutine(WaitAndShowDeveloperPanel());
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_waitAndShowCoroutine != null)
            {
                StopCoroutine(_waitAndShowCoroutine);
            }
        }

        private IEnumerator WaitAndShowDeveloperPanel()
        {
            yield return new WaitForSeconds(_timeToOpen);
            _developerPanel.TogglePanel();
        }
    }
}
