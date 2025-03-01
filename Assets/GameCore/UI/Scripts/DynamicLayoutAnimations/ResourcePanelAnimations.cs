using UnityEngine;
using DG.Tweening;
using TMPro;

[System.Serializable]
public class ResourcePanelAnimations 
{
    private readonly float _fullFade = 0;
    private readonly float _fullVisible = 1;

    [Header("Count change")]
    [SerializeField] private float _changeCountTime = 0.5f;
    [SerializeField] private AnimationCurve _changeCountCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [Header("Fade animation")]
    [SerializeField] private float _fullFadeTime = 0.3f;
    [SerializeField] private AnimationCurve _fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [Header("Adjust animation")]
    [SerializeField] private float _moveTime = 0.3f;
    [SerializeField] private AnimationCurve _moveCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private int _currentNumber = 0;
    
    private CanvasGroup _canvasGroup;
    private TMP_Text _countText;

    private Tween _countChangeAnimation;
    private Tween _fadeAnimation;
    private Tween _adjustAnimation;

    public void Init(TMP_Text countText, CanvasGroup group)
    {
        this._countText = countText;
        this._canvasGroup = group;
    }
    public Tween StartTextAnimation(int count)
    {
        _countChangeAnimation?.Kill();
        _countChangeAnimation = DOVirtual.Int(_currentNumber, count, _changeCountTime, SetText).SetLink(_countText.gameObject);
        return _countChangeAnimation;
    }
    public void UpdateCount(int count)
    {
        this._currentNumber = count;
    }
    public Tween StartFadeAnimation(float to)
    {
        _fadeAnimation?.Kill();
        float fadeTime = Mathf.Abs(to - _canvasGroup.alpha);
        _fadeAnimation = _canvasGroup.DOFade(to, fadeTime).SetEase(_fadeCurve).SetLink(_canvasGroup.gameObject);
        return _fadeAnimation;
    }

    public Tween StartAdjustAnimation()
    {
        _adjustAnimation?.Kill();

        _adjustAnimation = _canvasGroup.transform.DOLocalMove(Vector3.zero, _moveTime)
                            .SetLink(_canvasGroup.gameObject)
                            .SetEase(_moveCurve);
        return _adjustAnimation;
    }

    private void SetText(int count)
    {
        _countText.text = count.ToString();
        _currentNumber = count;
    }

}
