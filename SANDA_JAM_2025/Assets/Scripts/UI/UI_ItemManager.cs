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

    void Start()
    {
        cursorFollower = FindObjectOfType<UICursorFollower>();
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
            Instantiate(instantiableObjects[selectedItemIndex], worldPos, Quaternion.identity);
        }
    }


}
