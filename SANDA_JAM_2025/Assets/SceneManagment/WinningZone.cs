using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningZone : MonoBehaviour
{
    [SerializeField] private string goalSfxName = "GoalSfx";
    private bool goalTriggered = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!goalTriggered && collision.CompareTag("Player"))
        {
            goalTriggered = true;

            GameManager.instance.goalReached = true;

            // Play win sound once
            SoundFXManager.instance.PlaySoundByName(goalSfxName, transform);
        }
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene found in Build Settings.");
        }
    }
}
