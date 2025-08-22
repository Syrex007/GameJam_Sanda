using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public GameObject[] instantiableObjects;
    [SerializeField] public int selectedItemIndex = -1;

    [Header("Panel donde se instanciarán los objetos")]
    [SerializeField] private RectTransform targetPanel;

    private UICursorFollower cursorFollower;

    public int itemsUsed = 0;

    void Start()
    {
        cursorFollower = FindObjectOfType<UICursorFollower>();
    }

    void Update()
    {
        // Loop through number keys (1–9, you can extend for 0 if needed)
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString())) // checks "1","2","3",...
            {
                
                SelectByHierarchyIndex(i);
            }
        }
    }

    private void SelectByHierarchyIndex(int hierarchyIndex)
    {
        UI_SelectableItem[] allItems = FindObjectsOfType<UI_SelectableItem>();

        foreach (var item in allItems)
        {
            if (item.hierarchyIndex == hierarchyIndex)
            {
                if (item.currentQuantity > 0) // only allow selection if available
                {
                    SelectItem(item.itemIndex);
                    item.SelectAnimation(); // Play selection animation
                    SoundFXManager.instance.PlaySoundByName("Damage1", gameObject.transform);
                }
                else
                {
                    // If no quantity left and it’s selected, deselect it
                    if (selectedItemIndex == item.itemIndex)
                        DeselectItem();
                }
                return;
            }
        }
    }

    public void SelectItem(int index)
    {
        selectedItemIndex = index;
        UpdateCursorFollower();
    }

    public void DeselectItem()
    {
        selectedItemIndex = -1;
        cursorFollower.Hide();
    }

    private void UpdateCursorFollower()
    {
        if (selectedItemIndex < 0 || selectedItemIndex >= instantiableObjects.Length)
        {
            cursorFollower.Hide();
            return;
        }

        GameObject obj = instantiableObjects[selectedItemIndex];
        Sprite sprite = null;

        if (obj != null)
        {
            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null) sprite = sr.sprite;

            var img = obj.GetComponent<Image>();
            if (img != null) sprite = img.sprite;
        }

        if (sprite != null)
            cursorFollower.Show(sprite);
        else
            cursorFollower.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectedItemIndex < 0) return;
        if (targetPanel == null) return;

        if (!RectTransformUtility.RectangleContainsScreenPoint(targetPanel, eventData.position, eventData.pressEventCamera))
            return;

        Camera cam = eventData.pressEventCamera;
        Vector3 worldPos;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(targetPanel, eventData.position, cam, out worldPos))
        {
            UI_SelectableItem[] allItems = FindObjectsOfType<UI_SelectableItem>();
            foreach (var item in allItems)
            {
                if (item.itemIndex == selectedItemIndex)
                {
                    if (item.currentQuantity > 0)
                    {
                        Instantiate(instantiableObjects[selectedItemIndex], worldPos, Quaternion.identity);
                        item.UseItem(1);
                        itemsUsed = itemsUsed + 1;
                    }
                    return;
                }
            }
        }
    }
}
