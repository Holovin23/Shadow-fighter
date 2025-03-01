using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Extensions
{
    public static SkinnedMeshRenderer DoBlendShape(this SkinnedMeshRenderer renderer, float endValue, float duration, AnimationCurve destroyCurve = null) => renderer.DoBlendShape(0, endValue, duration, destroyCurve);
    public static SkinnedMeshRenderer DoBlendShape(this SkinnedMeshRenderer renderer, float endValue, float duration, Ease destroyCurve) => renderer.DoBlendShape(0, endValue, duration, destroyCurve);

    public static SkinnedMeshRenderer DoBlendShape(this SkinnedMeshRenderer renderer, int blendShapeIndex, float endValue, float duration, AnimationCurve destroyCurve = null)
    {
        if (destroyCurve == null) destroyCurve = AnimationCurve.Linear(0, 0, 1, 1);

        var tween = DOTween.To(() => renderer.GetBlendShapeWeight(blendShapeIndex), x => renderer.SetBlendShapeWeight(blendShapeIndex, x), endValue, duration).SetAutoKill(true).SetEase(destroyCurve);
        tween.SetTarget(renderer);
        return renderer;
    }
    public static SkinnedMeshRenderer DoBlendShape(this SkinnedMeshRenderer renderer, int blendShapeIndex, float endValue, float duration, Ease destroyCurve)
    {
        var tween = DOTween.To(() => renderer.GetBlendShapeWeight(blendShapeIndex), x => renderer.SetBlendShapeWeight(blendShapeIndex, x), endValue, duration).SetAutoKill(true).SetEase(destroyCurve);
        tween.SetTarget(renderer);
        return renderer;
    }
}
