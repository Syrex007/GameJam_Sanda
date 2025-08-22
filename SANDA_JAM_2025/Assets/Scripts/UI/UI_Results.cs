using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_Results : MonoBehaviour
{
    private float timeTaken;
    private int stars;

    [SerializeField] private TMP_Text starsText;
    [SerializeField] private TMP_Text timeText;

    private UI_TweenEffects tweenEffects;
    private Vector2 finalPos;

    [SerializeField]private float startPosY = 300f; 

    void Start()
    {
        
       

       
        
    }

    private void OnEnable()
    {
        tweenEffects = GetComponent<UI_TweenEffects>();


        RectTransform rt = GetComponent<RectTransform>();
        finalPos = rt.anchoredPosition;
        tweenEffects = GetComponent<UI_TweenEffects>();
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
            int currentLevel = GameManager.instance.currentLevel;
            stars = GameManager.instance.stars[currentLevel];
            timeTaken = GameManager.instance.times[currentLevel];

            int minutes = (int)(timeTaken / 60);
            int seconds = (int)(timeTaken % 60);
            int hundredths = (int)((timeTaken * 100) % 100);

            starsText.text = "Stars: " + stars.ToString();
            timeText.text = string.Format("Time: {0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
        }
       
    }
}
