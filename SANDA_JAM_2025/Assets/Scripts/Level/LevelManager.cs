using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    private int levelIndex = 0; 

    [SerializeField] private GameObject transitionCanvas;
    private UI_TransitionCanvas transitionCanvasScript;
    private bool isTimerPaused = false;
    [SerializeField] private UI_ItemManager itemManager;

    private bool levelFinished = false;

    [Header("Restart Hold Settings")]
    private bool canHold = false;
    private bool isHoldingR = false;
    private float holdTime = 0f;
    [SerializeField] private float requiredHoldTime = 1f;

    [Header("Timer")]
    [SerializeField] private TMP_Text timerText;
    private float elapsedTime = 0f;


    [Header("Star Rating Settings")] //cambian por cada nivel
    [SerializeField] private float timeThreshold3Stars = 30f; // <= 30s
    [SerializeField] private float timeThreshold2Stars = 60f; // <= 60s
    [SerializeField] private int objectThreshold3Stars = 3;   // <= 3 objetos
    [SerializeField] private int objectThreshold2Stars = 5;   // <= 5 objetos

    private int usedItems = 0; // Puedes incrementar esto cuando el jugador use algo



    [Header("UI")]
    [SerializeField] private GameObject endResultsUI;

    void Start()
    {
        GameManager.instance.currentLevel = levelIndex;
        canHold = false;
        transitionCanvas.SetActive(true);
        transitionCanvasScript = transitionCanvas.GetComponent<UI_TransitionCanvas>();
        elapsedTime = 0f;
        UpdateTimerUI();
    }

    void Update()
    {
        if (GameManager.instance.goalReached && (!levelFinished))
        {
            LevelFinish();
            levelFinished = true;
        }

        if (!isTimerPaused)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }

        if (AnyKeyReleased())
            canHold = true;

        if (Input.GetKeyDown(KeyCode.R) && canHold)
        {
            isHoldingR = true;
            holdTime = 0f;
            transitionCanvasScript.FadeIn();
        }

        if (Input.GetKey(KeyCode.R) && isHoldingR)
        {
            holdTime += Time.deltaTime;
            if (holdTime >= requiredHoldTime)
            {
                isHoldingR = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            if (holdTime < requiredHoldTime)
                transitionCanvasScript.StopFade();

            isHoldingR = false;
            holdTime = 0f;
        }
    }


    private bool AnyKeyReleased()
    {
        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyUp(k))
                return true;
        }
        return false;
    }

    private void UpdateTimerUI()
    {
        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int hundredths = (int)((elapsedTime * 100) % 100);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
    }
    

    public void PauseTimer()
    {
        isTimerPaused = true;
    }

    public void ResumeTimer()
    {
        isTimerPaused = false;
    }

    private void LevelFinish()
    {
        usedItems = itemManager.itemsUsed;
        PauseTimer();
        GameManager.instance.stars[GameManager.instance.currentLevel] = CalculateStarRating();
        GameManager.instance.times[GameManager.instance.currentLevel] = elapsedTime;
        endResultsUI.SetActive(true);
    }

    public void LoadNextLevel() {
        GameManager.instance.goalReached = false;
        //cambiar para mas escenas
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private int CalculateStarRating()
    {
        int timeStars = 1;
        int objectStars = 1;

        // Evaluar por tiempo
        if (elapsedTime <= timeThreshold3Stars)
            timeStars = 3;
        else if (elapsedTime <= timeThreshold2Stars)
            timeStars = 2;

        // Evaluar por objetos usados
        if (usedItems <= objectThreshold3Stars)
            objectStars = 3;
        else if (usedItems <= objectThreshold2Stars)
            objectStars = 2;

        // Promediar (redondeado hacia abajo)
        int finalStars = Mathf.Min(timeStars, objectStars);
        return finalStars;
    }

}
