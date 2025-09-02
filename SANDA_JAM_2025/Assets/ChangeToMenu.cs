using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToMenu : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ReturnMenu()
    {
        Time.timeScale = 1f; // Start the game time
        GameManager.instance.isGamePaused = false;
        SoundFXManager.instance.StopSoundByName("Item1");
        SoundFXManager.instance.StopSoundByName("Item2");
        SoundFXManager.instance.StopSoundByName("Item3");
        SoundFXManager.instance.StopSoundByName("Extinguisher");
        SoundFXManager.instance.StopSoundByName("Tema1");
        SoundFXManager.instance.StopSoundByName("Tema2");
        SoundFXManager.instance.StopSoundByName("TemaMenu");

        GameManager.instance.goalReached = false;
        SoundFXManager.instance.StopAllSounds();
        SceneManager.LoadScene(0);
    }
}
