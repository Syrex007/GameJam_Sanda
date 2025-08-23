using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_Results : MonoBehaviour
{
    private float timeTaken;
    private int stars;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private GameObject[] starFills;
    [SerializeField] private TMP_Text timeText;

    private UI_TweenEffects tweenEffects;
    private Vector2 finalPos;

    [SerializeField] private float startPosY = 300f;

    void Start() { }

    private void OnEnable()
    {
        tweenEffects = GetComponent<UI_TweenEffects>();

        RectTransform rt = GetComponent<RectTransform>();
        finalPos = rt.anchoredPosition;

        tweenEffects.PlayMove(
            startPos: finalPos + new Vector2(0, startPosY),
            endPos: finalPos,
            duration: 0.5f,
            ease: Ease.OutCubic
        );
    }

    void Update()
    {
        if (GameManager.instance.goalReached)
        {
            stars = levelManager.currentStars;
            timeTaken = levelManager.elapsedTime;

            // Format and display time
            int minutes = (int)(timeTaken / 60);
            int seconds = (int)(timeTaken % 60);
            int hundredths = (int)((timeTaken * 100) % 100);
            timeText.text = string.Format("Time: {0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);

            // Update star fills
            for (int i = 0; i < starFills.Length; i++)
            {
                starFills[i].SetActive(i < stars);
            }
        }
    }
}
