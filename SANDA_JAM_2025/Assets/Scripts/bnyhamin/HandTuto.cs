using System.Collections;
using UnityEngine;

public class HandTuto : MonoBehaviour
{
    [Tooltip("Objeto que seleccionará inicialmente.")]
    public GameObject goItem;

    [Tooltip("Objeto que se desea instanciar al llegar al punto final.")]
    public GameObject goPrefabItemInstanciar;

    [Tooltip("Tiempo de espera al llegar al punto final antes de pasar al siguiente objeto.")]
    public float waitTimePointEnd;

    [Tooltip("Objeto siguiente que se desea activar al llegar al punto final.")]
    public GameObject goNextHandActive = null;    

    [Header("Configuración")]
    [Tooltip("Desde la animación activará este check cuando pase por el pull.")]
    public bool checkAnimItem = false;
    [Tooltip("Desde la animación activará este check cuando llegue al punto final para que el player se acerque.")]
    public bool checkMovePlayer = false;
    [Tooltip("Desde la animación activará este check para indicar que ya terminó y podría pasar al siguiente HandTuto.")]
    public bool checkInactive = false;
    private int countSelectItem = 0;
    private int countMoveItem = 0;
    private int countInactive = 0;

    public int changeRadius;


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
        if (countMoveItem == 0) {
            GameObject goInst = Instantiate(goPrefabItemInstanciar, transform);
            if (changeRadius > 0) goInst.GetComponent<Attractor>().Radius = changeRadius;
        } 
        checkMovePlayer = false;

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

   

}
