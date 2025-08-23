using UnityEngine;
using UnityEngine.UI;

public class UICursorFollower : MonoBehaviour
{
    public Canvas canvas;         
    public Image followerImage; 
    public Image areaFollowerImage;   

    private RectTransform followerRect;
    private RectTransform areaFollowerRect;


    void Start()
    {
        if (followerImage != null)
            followerRect = followerImage.GetComponent<RectTransform>();

        if (areaFollowerImage != null)
            areaFollowerRect = areaFollowerImage.GetComponent<RectTransform>();
            
        

        if (canvas == null)
                canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (followerImage == null || !followerImage.gameObject.activeSelf && 
            (areaFollowerImage == null || !areaFollowerImage.gameObject.activeSelf)) return;
        if (!GameManager.instance.isGamePaused)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out localPos
            );

            followerRect.localPosition = localPos;
            
            if (areaFollowerRect != null)
                areaFollowerRect.localPosition = localPos;
        }
        
    }


    public void Show(Sprite sprite, Sprite areaSprite)
    {
        followerImage.sprite = sprite;
        followerImage.enabled = true;

        Color c = followerImage.color;
        c.a = 0.5f;
        followerImage.color = c;

        if (areaSprite != null)
        {
            areaFollowerImage.sprite = areaSprite;
            areaFollowerImage.enabled = true;
            Color a = areaFollowerImage.color;
            a.a = 0.3f;
            areaFollowerImage.color = a;

        }

    }

    public void Hide()
    {
        followerImage.enabled = false;
        if (areaFollowerImage != null)
            areaFollowerImage.enabled = false;
    }
}
