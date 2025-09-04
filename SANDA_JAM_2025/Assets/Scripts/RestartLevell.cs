using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevell : MonoBehaviour
{
    public void ReloadScene()
    {
         // Aseguramos que el GameManager exista y que no se haya completado el nivel
        if (GameManager.instance != null && !GameManager.instance.goalReached)
        {
            // Detenemos todos los sonidos
            if (SoundFXManager.instance != null)
                SoundFXManager.instance.StopAllSounds();

            // Reiniciamos las variables necesarias del GameManager
            GameManager.instance.goalReached = false;

            // Recargamos la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
}
