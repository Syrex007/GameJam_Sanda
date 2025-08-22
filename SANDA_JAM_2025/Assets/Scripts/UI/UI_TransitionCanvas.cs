using UnityEngine;
using DG.Tweening;
public class UI_TransitionCanvas : MonoBehaviour
{
    private UI_TweenEffects tweenEffects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tweenEffects = GetComponent<UI_TweenEffects>();
        FadeOut(); //Start with fade out effect
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn() {
        tweenEffects.PlayFade(0f, 1f, 0.5f, Ease.InOutQuad, () =>
        {
            Debug.Log("Transition complete");
        });
    }
    
    public void FadeOut()
    {
        tweenEffects.PlayFade(1f, 0f, 0.5f, Ease.InOutQuad, () =>
        {
            Debug.Log("Transition complete");
        });
    }
    public void StopFade()
    {
        tweenEffects.KillCurrentTween();
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0f; //Reset alpha to 0
        }
        Debug.Log("Fade stopped and reset to 0");
    }
}
