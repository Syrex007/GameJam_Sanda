using UnityEngine;
using DG.Tweening;

public class UI_TweenEffects : MonoBehaviour
{
    private Tween currentTween;

    public void PlayPop(
        Vector3 startScale,
        Vector3 popScale,
        Vector3 endScale,
        float duration,
        float popRatio,
        Ease popEase,
        Ease settleEase,
        System.Action onComplete = null)
    {
        KillCurrentTween();
        transform.localScale = startScale;

        float popTime = duration * popRatio;
        float settleTime = duration - popTime;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(popScale, popTime).SetEase(popEase));
        seq.Append(transform.DOScale(endScale, settleTime).SetEase(settleEase));
        if (onComplete != null) seq.OnComplete(() => onComplete());

        currentTween = seq;
    }

    public void PlayMove(
        Vector3 startPos,
        Vector3 endPos,
        float duration,
        Ease ease,
        System.Action onComplete = null)
    {
        KillCurrentTween();
        transform.localPosition = startPos;

        currentTween = transform.DOLocalMove(endPos, duration).SetEase(ease);
        if (onComplete != null) currentTween.OnComplete(() => onComplete());
    }

    public void PlayFade(
        float startAlpha,
        float endAlpha,
        float duration,
        Ease ease,
        System.Action onComplete = null)
    {
        KillCurrentTween();
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        cg.alpha = startAlpha;

        currentTween = cg.DOFade(endAlpha, duration).SetEase(ease);
        if (onComplete != null) currentTween.OnComplete(() => onComplete());
    }

    public void PlayRotate(
        Vector3 startRot,
        Vector3 endRot,
        float duration,
        Ease ease,
        System.Action onComplete = null)
    {
        KillCurrentTween();
        transform.localEulerAngles = startRot;

        currentTween = transform.DOLocalRotate(endRot, duration).SetEase(ease);
        if (onComplete != null) currentTween.OnComplete(() => onComplete());
    }

    private void KillCurrentTween()
    {
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();
    }
}
