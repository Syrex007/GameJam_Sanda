using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // <- if you use TextMeshPro

public class UI_SelectableItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public int itemIndex;

    private UI_ItemManager itemManager;
    private UI_TweenEffects effects;

    [SerializeField] private int startQuantity;
    [SerializeField] public int currentQuantity;

    [Header("UI References")]
    [SerializeField] private TMP_Text indexText;     // Show the item index
    [SerializeField] private TMP_Text quantityText;  // Show the quantity left

    void Start()
    {
        currentQuantity = startQuantity;
        itemManager = FindObjectOfType<UI_ItemManager>();
        effects = GetComponent<UI_TweenEffects>();

        UpdateUI(); // initialize UI
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (currentQuantity <= 0)
        {
            if (itemManager != null && itemManager.selectedItemIndex == itemIndex)
            {
                itemManager.DeselectItem();
            }
            return;
        }

        if (itemManager != null)
        {
            if (itemIndex == itemManager.selectedItemIndex)
            {
                itemManager.DeselectItem();
            }
            else
            {
                itemManager.SelectItem(itemIndex);

                effects.PlayPop(
                    startScale: Vector3.one * 0.9f,
                    popScale: Vector3.one * 1.2f,
                    endScale: Vector3.one,
                    duration: 0.4f,
                    popRatio: 0.4f,
                    popEase: DG.Tweening.Ease.OutBack,
                    settleEase: DG.Tweening.Ease.OutQuad
                );
            }
        }
    }

    public void UseItem(int amount = 1)
    {
        currentQuantity -= amount;
        if (currentQuantity < 0) currentQuantity = 0;

        UpdateUI();

        if (currentQuantity == 0 && itemManager != null && itemManager.selectedItemIndex == itemIndex)
        {
            itemManager.DeselectItem();
        }
    }

    private void UpdateUI()
    {
        if (indexText != null)
            indexText.text = $"{itemIndex+1}";

        if (quantityText != null)
            quantityText.text = $"x{currentQuantity}";
    }
}
