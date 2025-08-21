using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class UI_ItemText : MonoBehaviour
{
    private TMP_Text itemText;
    private UI_SelectableItem item;
    void Start()
    {
        itemText = GetComponent<TMP_Text>();
        item = GetComponentInParent<UI_SelectableItem>();
    }

    void Update()
    {
        itemText.text =  (item.itemIndex +1).ToString();
    }
}
