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
