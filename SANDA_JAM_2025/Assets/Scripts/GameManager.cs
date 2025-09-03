using System;
using System.Collections;
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
        
        /////
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

        private void OnDestroy()
    {
        ////
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        string soundToPlay = "";

        switch (sceneName)
        {
            // Escenas del menú
            case "JulioSceneMenu2":
            case "Credits":
                soundToPlay = "TemaMenu";
                break;

            // Niveles del 0 al 7
            case "Nivel_0":
            case "Nivel_1":
            case "Nivel_2":
            case "Nivel_3":
            case "Nivel_4":
            case "Nivel_5":
            case "Nivel_6":
            case "Nivel_7":
                soundToPlay = "Tema1";
                break;

            // Niveles del 8 al 14
            case "Nivel_8":
            case "Nivel_9":
            case "Nivel_10":
            case "Nivel_11":
            case "Nivel_12":
            case "Nivel_13":
            case "Nivel_14":
                soundToPlay = "Tema2";
                break;
        }

        // Si encontramos un tema, lo reproducimos si no está sonando ya
        if (!string.IsNullOrEmpty(soundToPlay) && !SoundFXManager.instance.IsSoundPlaying(soundToPlay))
        {
            SoundFXManager.instance.PlaySoundByName(soundToPlay, transform, 1f, 1f, true);
            Debug.Log($"Reproduciendo música: {soundToPlay} | Escena: {sceneName}");
        }
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

}
