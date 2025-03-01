using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UI.UIElements.Layouts;

[System.Serializable]
public class DynamicLayoutAnimations
{
    [Header("Appear settings")]
    [SerializeField] private float _appearDuration = 0.3f;
    [SerializeField] private Vector3 _fromScale = Vector3.zero;
    [SerializeField] private AnimationCurve _appearCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private Tween _currentAppearAnimation;

    public Tween ShowElement(ResourcePanelUI animationTarget)
    {
        Vector3 defaultScale = animationTarget.transform.localScale;
        _currentAppearAnimation = animationTarget.transform.DOScale(defaultScale, _appearDuration)
            .From(_fromScale)
            .SetEase(_appearCurve)
            .SetLink(animationTarget.gameObject);
        return _currentAppearAnimation;   
    }
}