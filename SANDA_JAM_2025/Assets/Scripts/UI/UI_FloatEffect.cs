using UnityEngine;

public class UI_FloatEffect : MonoBehaviour
{
    private UI_TweenEffects tweenEffects;

    void Start()
    {
        tweenEffects = GetComponent<UI_TweenEffects>();
        tweenEffects.PlayFloatEffectSimple();
    }
}


