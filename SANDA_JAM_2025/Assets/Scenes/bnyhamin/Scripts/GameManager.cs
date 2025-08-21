using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int level;
    public int estrellitas;
    public float tiempoTotal;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void SetNextLevel()
    {
        this.level = level+1;
        print(this.level);
        if (this.level == 2) //si es el último nivel
        {
            HighScore();
        }
        else
        {
            SceneManager.LoadScene(this.level);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        tiempoTotal += Time.deltaTime;
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
}
