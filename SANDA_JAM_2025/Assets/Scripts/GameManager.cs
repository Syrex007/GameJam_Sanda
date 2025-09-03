using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Game State")]
    public bool isGamePaused = false;
    public int currentLevel;
    public bool goalReached;

    [Header("Level Stats")] 
    //public float[] scores;//para cada nivel
    public int[] stars;//para cada nivel
    public float[] times;//para cada nivel

    public float tiempoTotal;

    private void Awake()
    {
        
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedVerifySound(scene.name));

    }

    public void SetLevel(int level)
    {
        this.currentLevel = level;
    }

    public void SetNextLevel()
    {
        this.currentLevel = currentLevel + 1;
        print(this.currentLevel);
        if (this.currentLevel == 2) //si es el �ltimo nivel
        {
            HighScore();
        }
        else
        {
            SceneManager.LoadScene(this.currentLevel);
        }
    }

    public IEnumerator nextScene()
    {
        yield return new WaitForSeconds(3);
        //LeanTween        
        SetNextLevel();
        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void HighScore()
    {
        calcularTiempoTotal();

        int totalMilliseconds = Mathf.FloorToInt(tiempoTotal * 1000f); // convertir a milisegundos

        int minutes = totalMilliseconds / 60000;
        int seconds = (totalMilliseconds % 60000) / 1000;
        int milliseconds = (totalMilliseconds % 1000) / 10; // cent�simas (2 d�gitos)

        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", minutes, seconds, milliseconds);
        Debug.Log("Tiempo total: " + formattedTime);
    }

    public void calcularTiempoTotal()
    {
        tiempoTotal = 0f;
        for (int i = 0; i < times.Length; i++)
        {
            tiempoTotal += times[i];
        }
    }

    private IEnumerator DelayedVerifySound(string sceneName)
    {
        // Esperar hasta el final del frame para asegurarnos de que
        // todos los GameObjects de la escena ya están cargados y activos
        yield return new WaitForEndOfFrame();

        verifySound(sceneName);
    }

    private void verifySound(string sceneName)
    {
        string soundToPlay = "";
        print("ENTRAAA Nombre de escena:" + sceneName);
        // Decidir qué música reproducir
        switch (sceneName)
        {
            case "JulioSceneMenu2":
                soundToPlay = "TemaMenu";
                SoundFXManager.instance.StopAllSoundsMusicEffect();
                //SoundFXManager.instance.StopSoundByName("Tema1");
                //SoundFXManager.instance.StopSoundByName("Tema2");
                break;
            case "Credits":
                soundToPlay = "TemaMenu";
                SoundFXManager.instance.StopAllSoundsMusicEffect();
                //SoundFXManager.instance.StopSoundByName("Tema1");
                //SoundFXManager.instance.StopSoundByName("Tema2");
                break;
            case "Nivel_0":
            case "Nivel_1":
            case "Nivel_2":
            case "Nivel_3":
            case "Nivel_4":
            case "Nivel_5":
            case "Nivel_6":
            case "Nivel_7":
                soundToPlay = "Tema1";
                //SoundFXManager.instance.StopAllSoundsMusicEffect();
                //SoundFXManager.instance.StopSoundByName("TemaMenu");
                //SoundFXManager.instance.StopSoundByName("Tema2");
                break;
            case "Nivel_8":
            case "Nivel_9":
            case "Nivel_10":
            case "Nivel_11":
            case "Nivel_12":
            case "Nivel_13":
            case "Nivel_14":
                soundToPlay = "Tema2";
                //SoundFXManager.instance.StopSoundByName("TemaMenu");
                //SoundFXManager.instance.StopSoundByName("Tema1");
                break;
            default:
                // Por defecto dejamos el TemaMenu
                soundToPlay = "TemaMenu";
                break;
        }

        // Solo cambiamos la música si NO está sonando la correcta
        //if (!SoundFXManager.instance.IsSoundPlayingExisting(soundToPlay)  )
        /*if (!SoundFXManager.instance.gameObject.GetComponentsInChildren<AudioSource>(true).Any(a => a.clip && a.clip.name == soundToPlay && a.gameObject.activeSelf))
        {
            print(sceneName + " " + soundToPlay);
            SoundFXManager.instance.PlaySoundByName(soundToPlay, transform, 1f, 1f, true, false);
            
        }*/
        // Verificamos si ya se está reproduciendo la canción correcta
        bool alreadyPlaying = SoundFXManager.instance
            .gameObject
            .GetComponentsInChildren<AudioSource>(true)
            .Any(a => a.clip && a.clip.name == soundToPlay && a.isPlaying);

        if (alreadyPlaying)
        {
            // Ya está sonando la canción correcta, no hacemos nada
            print($"[{sceneName}] La música '{soundToPlay}' ya está sonando, no la reinicio.");
            return;
        }

        // Si llegamos aquí, significa que la canción actual es distinta → detenemos todo
        SoundFXManager.instance.StopAllSoundsMusicEffect();

        // Ahora reproducimos la canción correspondiente
        print($"{sceneName} → Reproduciendo {soundToPlay}");
        SoundFXManager.instance.PlaySoundByName(soundToPlay, transform, 1f, 1f, true, false);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        StartCoroutine(DelayedVerifySound(newScene.name));
    }

}
