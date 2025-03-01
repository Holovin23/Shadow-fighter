using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class SelectionPanelAnimations
{

    [SerializeField] private float duration = 0.3f;


    [Header("Icon animation")]
    [SerializeField] private Image _icon;
    [SerializeField] private Vector2 _selectedIconPivot = new Vector2(0.5f,0);
    [SerializeField] private Vector3 _unselectedIconPivot = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector3 _selectedIconScale = Vector3.one * 1.15f;
    [SerializeField] private Vector3 _unselectedIconScale = Vector3.one;
    [SerializeField] private AnimationCurve _iconCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("backgrond animation")]
    [SerializeField] private Image _background;
    [SerializeField] private Color _selectedBGColor = new Color(0,0,0.5f,1);
    [SerializeField] private Color _unselectedBGColor = Color.blue;
    [SerializeField] private Vector3 _selectedBGScale = Vector3.one * 1.15f;
    [SerializeField] private Vector3 _unselectedBGScale = Vector3.one;
    [SerializeField] private AnimationCurve _backgroundCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Text animation")]
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Vector3 _selectedTextScale = Vector3.one;
    [SerializeField] private Vector3 _unselectedTextScale = Vector3.zero;
    [SerializeField] private AnimationCurve _textCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private Tween _currentAnimation;

    private RectTransform Layout { get { return _background.rectTransform.parent as RectTransform; } }


    public void Select()
    {
        this._currentAnimation?.Kill();
        Sequence currentAnimation = DOTween.Sequence();
        currentAnimation.Join(DoBGColor(_selectedBGColor));
        currentAnimation.Join(DOSelectBGScale(_selectedBGScale));
        currentAnimation.Join(DoIconPivot(_selectedIconPivot));
        currentAnimation.Join(DoIconSelectScale(_selectedIconScale));
        currentAnimation.Join(DoSelectTextScale(_selectedTextScale));
        this._currentAnimation = currentAnimation;
    }

    public void Unselect()
    {
        this._currentAnimation?.Kill();
        Sequence currentAnimation = DOTween.Sequence();
        currentAnimation.Join(DoBGColor(_unselectedBGColor));
        currentAnimation.Join(DOSelectBGScale(_unselectedBGScale));
        currentAnimation.Join(DoIconPivot(_unselectedIconPivot));
        currentAnimation.Join(DoIconSelectScale(_unselectedIconScale));
        currentAnimation.Join(DoSelectTextScale(_unselectedTextScale));
        this._currentAnimation = currentAnimation;
    }

    private Tween DoBGColor(Color color)
    {
        return _background.DOColor(color, duration).SetLink(_background.gameObject).SetEase(_backgroundCurve);
    }
    private Tween DOSelectBGScale(Vector3 scale)
    {
        return _background.rectTransform.DOScale(scale, duration).SetLink(_background.gameObject).SetEase(_backgroundCurve).OnUpdate(() =>
        {
            LayoutRebuilder.MarkLayoutForRebuild(Layout);
        });
    }
    private Tween DoIconPivot(Vector2 pivot)
    {
        return _icon.rectTransform.DOPivot(pivot,duration).SetEase(_iconCurve).SetLink(_icon.gameObject);
        //return icon.rectTransform.DOAnchorPos(Vector3.zero,duration).SetEase(iconCurve).SetLink(icon.gameObject);
    }
    private Tween DoIconSelectScale(Vector3 scale)
    {
        return _icon.rectTransform.DOScale(scale, duration).SetEase(_iconCurve).SetLink(_icon.gameObject);
    }
    private Tween DoSelectTextScale(Vector3 scale)
    {
        return _text.rectTransform.DOScale(scale, duration).SetEase(_textCurve).SetLink(_text.gameObject);
    }
}
