using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void SetVolume(float sliderVaue)
    {
        mixer.SetFloat("SongVolume", Mathf.Log10(sliderVaue) * 20);
    }

    public void SetVolumeSFX(float sliderVaue)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(sliderVaue) * 20);
    }
}
