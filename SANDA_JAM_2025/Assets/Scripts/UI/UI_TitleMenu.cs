using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_TitleMenu : MonoBehaviour
{
    private UI_TweenEffects tweenEffects;

    

    void Start()
    {
        tweenEffects = GetComponent<UI_TweenEffects>();
        tweenEffects.PlayFloatEffect();
        SoundFXManager.instance.PlaySoundByName("TemaMenu", transform);
    }

    public void goToLevel()
    {
        // Stop menu music before loading the scene
        SoundFXManager.instance.StopSoundByName("TemaMenu");

        SceneManager.LoadScene("Nivel_0");
    }
}
