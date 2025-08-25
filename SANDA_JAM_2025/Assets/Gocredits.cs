using UnityEngine;
using UnityEngine.SceneManagement;

public class Gocredits : MonoBehaviour
{
    public void GoToCredits()
    {
        SoundFXManager.instance.StopSoundByName("TemaMenu");

        SceneManager.LoadScene("Credits");
    }
}
