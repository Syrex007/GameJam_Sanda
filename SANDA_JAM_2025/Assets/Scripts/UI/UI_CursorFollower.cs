using UnityEngine;
using UnityEngine.UI;

public class UICursorFollower : MonoBehaviour
{
    public Canvas canvas;         
    public Image followerImage;  

    private RectTransform followerRect;

    void Start()
    {
        if (followerImage != null)
            followerRect = followerImage.GetComponent<RectTransform>();

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (followerImage == null || !followerImage.gameObject.activeSelf) return;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out localPos
        );

        followerRect.localPosition = localPos;
    }

    
    public void Show(Sprite sprite)
    {
        followerImage.sprite = sprite;
        followerImage.enabled = true;

        Color c = followerImage.color;  
        c.a = 0.5f;                      
        followerImage.color = c;         
    }

    
    public void Hide()
    {
        followerImage.enabled = false;
    }
}
