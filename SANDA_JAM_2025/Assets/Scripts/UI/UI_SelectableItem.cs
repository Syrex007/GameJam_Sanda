using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

public class UI_SelectableItem :  MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool mouseHeldOnThis = false; 

    [HideInInspector] public int hierarchyIndex;
    [SerializeField] public int itemIndex;

    [Header("Active Effect Settings")]
    public UnityEvent onActiveUse;
    public UnityEvent onReleaseUse;
    [SerializeField] private float depleteRate = 1f;
    [SerializeField] public bool activeItem = false;

    [SerializeField] private UI_ItemManager itemManager;
    private UI_TweenEffects effects;

    [SerializeField] public int startQuantity;
    [SerializeField] public int currentQuantity;

    [Header("UI References")]
    [SerializeField] private TMP_Text hierarchyText;
    [SerializeField] private TMP_Text quantityText;

    private KeyCode myKey; // Store assigned number key
    private bool keyHeld = false;

    private bool activeUsed = false;
    void Start()
    {
        currentQuantity = startQuantity;
        itemManager = FindObjectOfType<UI_ItemManager>();
        effects = GetComponent<UI_TweenEffects>();
        myKey = KeyCode.Alpha0 + (transform.GetSiblingIndex() + 1);
        UpdateUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        // Ignore clicks for active items — use OnPointerDown/Up instead
        if (activeItem) return;

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
                SelectAnimation();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || !activeItem || currentQuantity <= 0)
            return;

        itemManager?.SelectItem(itemIndex);
        SelectAnimation();
        mouseHeldOnThis = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left || !mouseHeldOnThis)
            return;

        itemManager?.DeselectItem();
        mouseHeldOnThis = false;
    }

    public void SelectAnimation()
    {
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
        if (hierarchyText != null)
            hierarchyText.text = $"{transform.GetSiblingIndex() + 1}";

        if (quantityText != null)
            quantityText.text = $"x{currentQuantity}";
    }

    private void Update()
    {
        if (activeItem && currentQuantity == 0 && !activeUsed)
        {
            itemManager.itemsUsed++;
            activeUsed = true;
        }
        hierarchyIndex = transform.GetSiblingIndex() + 1;
        UpdateUI();

        if (itemManager == null || currentQuantity <= 0)
        {
            onReleaseUse?.Invoke();
            return;
        }
            

        if (activeItem && itemManager.selectedItemIndex == itemIndex && currentQuantity > 0)
        {
            onActiveUse?.Invoke();
            currentQuantity -= Mathf.CeilToInt(depleteRate * Time.deltaTime);

            if (currentQuantity <= 0)
            {
                currentQuantity = 0;
                itemManager.DeselectItem();
            }

            UpdateUI();
        }
        else
        {
            onReleaseUse?.Invoke();
        }

    }

}
