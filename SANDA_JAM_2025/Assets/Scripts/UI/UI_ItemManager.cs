using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public GameObject[] instantiableObjects;
    [SerializeField] public int selectedItemIndex = -1;
    private KeyCode selectedKey;
    private bool keyHeldForActiveItem;



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

        // Deselect the item if key is released and it’s an active item
        if (keyHeldForActiveItem && Input.GetKeyUp(selectedKey))
        {
            DeselectItem();
            keyHeldForActiveItem = false;
        }

    }

    private void SelectByHierarchyIndex(int hierarchyIndex)
    {
        UI_SelectableItem[] allItems = FindObjectsOfType<UI_SelectableItem>();

        foreach (var item in allItems)
        {
            if (item.hierarchyIndex == hierarchyIndex)
            {
                if (item.currentQuantity > 0)
                {
                    SelectItem(item.itemIndex);
                    item.SelectAnimation();
                    SoundFXManager.instance.PlaySoundByName("Damage1", gameObject.transform);

                    if (item.activeItem)
                    {
                        selectedKey = (KeyCode)(KeyCode.Alpha0 + hierarchyIndex);
                        keyHeldForActiveItem = true;
                    }
                    else
                    {
                        keyHeldForActiveItem = false;
                    }
                }
                else
                {
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
        

        if ((selectedItemIndex < 0 || selectedItemIndex >= instantiableObjects.Length)||(selectedItemIndex == 3)) //extintor
        {
            cursorFollower.Hide();
            return;
        }

        GameObject obj = instantiableObjects[selectedItemIndex];
        Sprite sprite = null;
        Sprite areaSprite = null;

        if (obj != null)
        {
            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null) sprite = sr.sprite;

            var img = obj.GetComponent<Image>();
            if (img != null) sprite = img.sprite;

            if (obj.transform.childCount > 0)
            {
                foreach (Transform child in obj.transform)
                {
                    var childSR = child.GetComponent<SpriteRenderer>();

                    if (childSR != null)
                    {
                        areaSprite = childSR.sprite;
                        ResizeAreaFollower(child.localScale.x);

                        break;
                    }    

                    var childImg = child.GetComponent<Image>();
                    if (childImg != null)
                    {
                        areaSprite = childImg.sprite;
                        ResizeAreaFollower(child.localScale.x);
                        break;
                    }
                    
                    
                }
            }
        }

        if (sprite != null || areaSprite != null)
            cursorFollower.Show(sprite, areaSprite);
        else
            cursorFollower.Hide();
    }

    private void ResizeAreaFollower(float objectScale)
    {
        if (cursorFollower.areaFollowerImage != null)
        {
            RectTransform areaRect = cursorFollower.areaFollowerImage.GetComponent<RectTransform>();

            // Tamaño base = 100x100, multiplicador = 55 * escala del objeto
            float finalSize = objectScale * 55f;
            areaRect.sizeDelta = new Vector2(finalSize, finalSize);
        }
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
