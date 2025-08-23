using UnityEngine;
using UnityEngine.UI;

public class Items_ActiveBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Slider activeBarSlider;
    [SerializeField] UI_SelectableItem selectableItem;
    [SerializeField] bool onHampster = false; 
    void Start()
    {
        activeBarSlider.maxValue = selectableItem.startQuantity;
    }

    // Update is called once per frame
    void Update()
    {
        activeBarSlider.value = selectableItem.currentQuantity;

        if (onHampster && (selectableItem.currentQuantity == selectableItem.startQuantity))
        {
            activeBarSlider.gameObject.SetActive(false);
            print("sigma");
        }
        else
        {
            if (selectableItem.currentQuantity <= 0)
            {
                activeBarSlider.gameObject.SetActive(false);
            }
            else
            {
                activeBarSlider.gameObject.SetActive(true);
            }
        }
        
    }
}
