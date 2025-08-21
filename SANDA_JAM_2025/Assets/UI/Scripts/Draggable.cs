using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Draggable : MonoBehaviour
{
    [SerializeField] private bool isDragging = false;
    [SerializeField] private bool isPlaced = false;
    private Vector3 offset;
    private Vector3 initialPosition;

    private bool isOverDropZone = false;

    void Start()
    {
        initialPosition = transform.position;
        Attractor attractor = GetComponent<Attractor>();
        if (attractor != null)
            attractor.enabled = false;
    }

    void OnMouseDown()
    {
        if (isPlaced) return;

        isDragging = true;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
    }

    void OnMouseDrag()
    {
        if (isDragging && !isPlaced)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;
        }
    }

    void OnMouseUp()
    {
        if (isPlaced) return;

        isDragging = false;

        if (isOverDropZone)
        {
            isPlaced = true;

            Attractor attractor = GetComponent<Attractor>();
            if (attractor != null)
                attractor.enabled = true;
        }
        else
        {
            // Volver al punto inicial
            transform.position = initialPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DropAble"))
        {
            isOverDropZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DropAble"))
        {
            isOverDropZone = false;
        }
    }
}
