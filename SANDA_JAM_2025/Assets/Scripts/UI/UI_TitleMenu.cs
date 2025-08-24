using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TitleMenu : MonoBehaviour
{
    private UI_TweenEffects tweenEffects;

    void Start()
    {
        tweenEffects = GetComponent<UI_TweenEffects>();
        tweenEffects.PlayFloatEffect();
    }

    public void goToLevel()
    {
        SceneManager.LoadScene("Nivel_0");
    }
}
