using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandTuto : MonoBehaviour
{
    [Tooltip("Objeto que seleccionar� inicialmente.")]
    public GameObject goItem;

    [Tooltip("Objeto que se desea instanciar al llegar al punto final.")]
    public GameObject goPrefabItemInstanciar;

    [Tooltip("Tiempo de espera al llegar al punto final antes de pasar al siguiente objeto.")]
    public float waitTimePointEnd;

    [Tooltip("Objeto siguiente que se desea activar al llegar al punto final.")]
    public GameObject goNextHandActive = null;    

    [Header("Configuraci�n")]
    [Tooltip("Desde la animaci�n activar� este check cuando pase por el pull.")]
    public bool checkAnimItem = false;
    [Tooltip("Desde la animaci�n activar� este check cuando llegue al punto final para que el player se acerque.")]
    public bool checkMovePlayer = false;
    [Tooltip("Desde la animaci�n activar� este check para indicar que ya termin� y podr�a pasar al siguiente HandTuto.")]
    public bool checkInactive = false;
    [Tooltip("Desde la animacion dispara al metodo setActiveOtherGOActiveEventTrigger() para activar los eventos de otro gameobject.")]
    public bool checkGOActiveEventTrigge = false;
    private int countSelectItem = 0;
    private int countMoveItem = 0;
    private int countInactive = 0;
    private int countActiveEventTrigger = 0;

    public int changeRadius;
    public GameObject[] othersGOActiveEventTrigger;

    // Update is called once per frame
    void Update()
    {
        if (checkAnimItem)
        {
            
            selectItem();
            countSelectItem++;

        }

        if (checkMovePlayer)
        {
            instanciarItem();
            countMoveItem++;
        }

        
        if (checkInactive)
        {
            StartCoroutine(waitInactive());
            countInactive++;
        }

        if (checkGOActiveEventTrigge)
        {
            setActiveOtherGOActiveEventTrigger();
            countActiveEventTrigger++;
        }

    }


    


    public void selectItem() {
        
        if (countSelectItem == 0 && goItem != null)
        {
            var selectable = goItem.GetComponent<UI_SelectableItem>();
            if (selectable != null)
            {
                selectable.SelectAnimation();
                selectable.UseItem();
            }
        }
        checkAnimItem = false;

    }

public void instanciarItem()
{
    if (countMoveItem == 0)
    {
        GameObject goInst = Instantiate(goPrefabItemInstanciar, transform);

        if (changeRadius > 0)
            goInst.GetComponent<Attractor>().Radius = changeRadius;

        countMoveItem++; // Aumentamos aquí para marcar que ya se instanció
        checkMovePlayer = false; // Desactivamos la bandera aquí
    }
}


    public void callwaitInactive() => StartCoroutine(waitInactive());

    IEnumerator waitInactive()
    {
        if(countInactive == 0)
        {
            checkInactive = false;
            yield return new WaitForSeconds(waitTimePointEnd);
            setActiveSimulated(false);
            gameObject.SetActive(false);
            if (goNextHandActive) goNextHandActive.SetActive(true);
            
        }
        
        
    }

    public void setActiveSimulated(bool state)
    {

        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().simulated = state;
    }

    public void InstanciarItemUI_SinParametros()
    {
        if (countMoveItem != 0)
            return;

        // 1. Obtener la posición del mouse EN PANTALLA
        Vector3 mousePos = Input.mousePosition;

        // 2. Convertir a coordenadas del mundo 2D
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        // MUY IMPORTANTE: en 2D, el z debe ser 0
        worldPos.z = 0;

        // 3. Instanciar en esa posición
        GameObject goInst = Instantiate(goPrefabItemInstanciar, worldPos, Quaternion.identity);

        // (Opcional) Ajustes extra
        if (changeRadius > 0)
            goInst.GetComponent<Attractor>().Radius = changeRadius;

        countMoveItem++;
        checkMovePlayer = false;
    }



    /*public void InstanciarItemUI_SinParametros()
    {
        // Tomar datos del último pointer del sistema
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition; // funciona en PC y móvil

        Ray ray = Camera.main.ScreenPointToRay(pointerData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (countMoveItem == 0)
            {
                GameObject goInst = Instantiate(goPrefabItemInstanciar, hit.point, Quaternion.identity);

                if (changeRadius > 0)
                    goInst.GetComponent<Attractor>().Radius = changeRadius;

                countMoveItem++;
                checkMovePlayer = false;
            }
        }
    }*/

    public void setActiveOtherGOActiveEventTrigger()
    {
        foreach(GameObject go in othersGOActiveEventTrigger)
        {
            go.GetComponent<Image>().raycastTarget = true;
            go.GetComponent<EventTrigger>().enabled = true;
        }

        
    }



}
