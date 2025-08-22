using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject pauseBackground;
    [SerializeField] GameObject pauseButtons;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.isGamePaused)
                UnpauseGame();
            else
                PauseGame();
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f; // Stop the game time
        ShowPauseUI();
        GameManager.instance.isGamePaused = true;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f; // Start the game time
        HidePauseUI();
        GameManager.instance.isGamePaused = false;
    }
    private void ShowPauseUI()
    {
        pauseBackground.SetActive(true);
        pauseButtons.SetActive(true);
    }
    private void HidePauseUI()
    {
        pauseBackground.SetActive(false); 
        pauseButtons.SetActive(false); 
    }
}
