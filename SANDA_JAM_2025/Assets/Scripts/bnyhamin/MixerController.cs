using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public void SetVolume(float sliderVaue)
    {
        mixer.SetFloat("SongVolume", Mathf.Log10(sliderVaue) * 20);
    }

}
