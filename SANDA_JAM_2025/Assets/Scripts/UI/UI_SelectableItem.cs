using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Needed for IPointerClickHandler
using DG.Tweening;             // If you want to call Ease directly

public class UI_SelectableItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public int itemIndex;
    [SerializeField] public GameObject instantiableObject;
    [SerializeField] public int initialQuantity;
    [SerializeField] public int currentQuantity;

    private Image image;
    private UI_TweenEffects effects;

    void Start()
    {
        image = GetComponent<Image>();
        effects = GetComponent<UI_TweenEffects>();
    }

    // Called automatically when the UI element is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnSelect();
        }
    }

    private void OnSelect()
    {
        // Example pop animation when selected
        effects.PlayPop(
            startScale: Vector3.one * 0.9f,    // slightly smaller to emphasize growth
            popScale: Vector3.one * 1.2f,      // overshoot
            endScale: Vector3.one,             // settle to normal
            duration: 0.4f,
            popRatio: 0.4f,
            popEase: Ease.OutBack,
            settleEase: Ease.OutQuad
        );
    }
}
