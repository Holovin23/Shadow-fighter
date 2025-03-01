using DG.Tweening;
using UnityEngine;

namespace UI.UIElements.Layouts
{
    public class DynamicLayoutItemHolder : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform; 

        /* OLD ANIMATION
        private float _adjustDuration;
        private Ease _adjustEase;
    
        private Tween _elementMoveTween;*/

        public ResourcePanelUI PlacedElement { get; private set; }
        public bool IsFree => PlacedElement == null;
        
        public void Init()
        {

        }
        /*OLD ANIMATION
         * public void Init(float adjustElementDuration, Ease adjustElementEase)
        {
            _adjustDuration = adjustElementDuration;
            _adjustEase = adjustElementEase;
        }*/
        
        public void PlaceElement(ResourcePanelUI element, bool stayInWorld = false)
        {
            ChangeElement(element, stayInWorld);
            element.SetPlaceholder(transform.GetSiblingIndex());
        }
        public void LoadElement(ResourcePanelUI element, bool stayInWorld = false)
        {
            ChangeElement(element, stayInWorld);
            element.LoadPlaceholder(transform.GetSiblingIndex());

        }
        public void RemovePlacedElement()
        {
            PlacedElement = null;
        }
        /* OLD ANIMATION
        public void StopElementAdjust()
        {
            _elementMoveTween?.Kill();
        }*/
        
        public void AdjustElementToHolder()
        {
            PlacedElement.SetPlaceholder(transform.GetSiblingIndex());

            return;
            /* OLD animation
            if (_adjustDuration == 0f)
             {
                 PlacedElement.transform.localPosition = Vector3.zero;
                 return;
             }

             StopElementAdjust();
             _elementMoveTween = PlacedElement.transform.DOLocalMove(Vector3.zero, _adjustDuration)
                 .SetEase(_adjustEase)
                 .SetLink(PlacedElement.gameObject);*/
        }

        public void Disable()
        {
            RemovePlacedElement();
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        private void ChangeElement(ResourcePanelUI element, bool stayInWorld = false)
        {
            _rectTransform.sizeDelta = element.transform.GetRectTransform().sizeDelta;
            PlacedElement = element;
            element.transform.SetParent(transform, stayInWorld);
        }
    }
}