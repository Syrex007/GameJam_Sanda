using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MenuLevelDisplay : MonoBehaviour
{
    [SerializeField] private GameObject[] starFills;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int levelIndex;

    void Update()
    {
        float time = GameManager.instance.times[levelIndex];
        int stars = GameManager.instance.stars[levelIndex];

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time - Mathf.Floor(time)) * 1000);

        timeText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";

        // Update stars
        for (int i = 0; i < starFills.Length; i++)
        {
            starFills[i].SetActive(i < stars);
        }
    }

    public void goToLevel()
    {
        SoundFXManager.instance.StopSoundByName("TemaMenu");
         if (levelIndex > 14)
        {
            SoundFXManager.instance.StopAllSounds();
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene("Nivel_" + levelIndex);
        }
    }
}
