using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SelectableItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public int itemIndex;
    private UI_ItemManager itemManager;
    private UI_TweenEffects effects;

    void Start()
    {
        itemManager = FindObjectOfType<UI_ItemManager>();
        effects = GetComponent<UI_TweenEffects>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (itemManager != null)
        {
            if (itemIndex == itemManager.selectedItemIndex)
            {
                //Si clicamos el mismo item deseleccionamos
                itemManager.DeselectItem();
            }
            else
            {
                //Seleccionar este item
                itemManager.SelectItem(itemIndex);

                //Animacin de feedback visual
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
}
