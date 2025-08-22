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
        if (this.currentLevel == 2) //si es el último nivel
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

    void HighScore()
    {
        //muestra canvas con los tiempos logrados 
        //se envía un link a una página para guardar los tiempos y estrellas recoleccionadas y obtener listado
        int minutos = Mathf.FloorToInt(tiempoTotal / 60f);
        int segundos = Mathf.FloorToInt(tiempoTotal % 60f);
        print("Tiempo:" + minutos +":"+ segundos);
        print($"Tiempo total: {minutos:D2}:{segundos:D2}");

    }
    void calcularTiempoTotal()
    {
        for(int i =0; i< times.Length; i++)
        {
            tiempoTotal= +times[i];
        }
    }
}
