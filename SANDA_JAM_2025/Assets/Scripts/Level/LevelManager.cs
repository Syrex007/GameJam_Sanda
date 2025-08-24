using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField]private int levelIndex = 0; 

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
    public float elapsedTime = 0f;

    public int currentStars;

    [Header("Star Rating Settings")] //cambian por cada nivel
    [SerializeField] private float timeStarsThreshold = 30f; // 30s
    [SerializeField] private int objectStarsThreshold = 4;   // 4 objetos

    private int usedItems = 0; // Puedes incrementar esto cuando el jugador use algo



    [Header("UI")]
    [SerializeField] private GameObject endResultsUI;
    [SerializeField] private TMP_Text starTimeText;
    [SerializeField] private TMP_Text starItemsText;

    void Start()
    {
        if (!SoundFXManager.instance.IsSoundPlaying("Tema2"))
        {
            SoundFXManager.instance.PlaySoundByName("Tema2", gameObject.transform,1f, 1f, true);
        }
            

        GameManager.instance.currentLevel = levelIndex;
        canHold = false;
        transitionCanvas.SetActive(true);
        transitionCanvasScript = transitionCanvas.GetComponent<UI_TransitionCanvas>();
        elapsedTime = 0f;
        UpdateTimerUI();
    }

    void Update()
    {
        // Update star requirement texts
        starItemsText.text = itemManager.itemsUsed + "/" + objectStarsThreshold + " items";
        starTimeText.text = timeStarsThreshold + " sec.";

        // Turn red if thresholds are surpassed
        if (itemManager.itemsUsed > objectStarsThreshold)
            starItemsText.color = Color.red;
        else
            starItemsText.color = Color.white;

        if (elapsedTime > timeStarsThreshold)
            starTimeText.color = Color.red;
        else
            starTimeText.color = Color.white;

        // Check for level finish
        if (GameManager.instance.goalReached && !levelFinished)
        {
            LevelFinish();
            levelFinished = true;
        }

        // Timer logic
        if (!isTimerPaused)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }

        // R to restart logic
        if (AnyKeyReleased())
            canHold = true;

        if (Input.GetKeyDown(KeyCode.R) && canHold&& !GameManager.instance.goalReached)
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

        currentStars = CalculateStarRating();
        if (GameManager.instance.stars[GameManager.instance.currentLevel] < currentStars)
        {
            GameManager.instance.stars[GameManager.instance.currentLevel] = currentStars;
        }
        if (((GameManager.instance.times[GameManager.instance.currentLevel] > elapsedTime)) || ((GameManager.instance.times[GameManager.instance.currentLevel] == 0))) {
            GameManager.instance.times[GameManager.instance.currentLevel] = elapsedTime;
        }
        
        endResultsUI.SetActive(true);
    }

    public void LoadNextLevel() {
        GameManager.instance.goalReached = false;
        //cambiar para mas escenas
        
    // Obtenemos el índice de la escena actual
    int currentIndex = SceneManager.GetActiveScene().buildIndex;

    // Obtenemos cuántas escenas hay en Build Settings
    int totalScenes = SceneManager.sceneCountInBuildSettings;

    // Verificamos si hay un nivel siguiente disponible
    if (currentIndex + 1 < totalScenes)
    {
        // Cargar siguiente nivel
        SceneManager.LoadScene(currentIndex + 1);
    }
    else
    {
        // Si estamos en el último nivel, mostramos mensaje y no hacemos nada más
        Debug.Log("¡Felicidades! Has completado todos los niveles.");
        
        // Opcional: podrías reiniciar desde la primera escena
        // SceneManager.LoadScene(0);
    }
    }
    private int CalculateStarRating()
    {
        int finalStars = 1;

        // Evaluar por tiempo
        if (elapsedTime <= timeStarsThreshold)
            finalStars++;
      

        // Evaluar por objetos usados
        if (usedItems <= objectStarsThreshold)
            finalStars++;
       

        return finalStars;
    }

}
