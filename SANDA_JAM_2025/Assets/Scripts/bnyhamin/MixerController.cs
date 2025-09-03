using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        float masterValue = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float songValue = PlayerPrefs.GetFloat("SongVolume", 1f);
        float sfxValue = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Buscar sliders SOLO si existen
        if (masterSlider == null)
        {
            GameObject obj = GameObject.Find("SliderVolumeMaster");
            if (obj != null) masterSlider = obj.GetComponent<Slider>();
        }

        if (musicSlider == null)
        {
            GameObject obj = GameObject.Find("SliderVolume");
            if (obj != null) musicSlider = obj.GetComponent<Slider>();
        }

        if (sfxSlider == null)
        {
            GameObject obj = GameObject.Find("SliderVolumeSFX");
            if (obj != null) sfxSlider = obj.GetComponent<Slider>();
        }

        // Si existen sliders, asignarles los valores guardados
        if (masterSlider != null) masterSlider.value = masterValue;
        if (musicSlider != null) musicSlider.value = songValue;
        if (sfxSlider != null) sfxSlider.value = sfxValue;

        // Aplicar los valores al mixer
        SetVolumeMaster(masterValue);
        SetVolume(songValue);
        SetVolumeSFX(sfxValue);
    }

    public void SetVolume(float sliderValue)
    {
        mixer.SetFloat("SongVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SongVolume", sliderValue);
    }

    public void SetVolumeSFX(float sliderValue)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }

    public void SetVolumeMaster(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
}
