using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UI_Player : MonoBehaviour
{
    public float minAngularVelocity = 100f;
    public float maxAngularVelocity = 300f;

    public float minMoveSpeed = 1f;
    public float maxMoveSpeed = 3f;

    private Rigidbody2D rb;

    private Vector2 dragStartPos;
    private float dragStartTime;
    private bool isDragging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ApplyRandomMotion();
    }

    void OnMouseDown()
    {
        isDragging = true;
        dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragStartTime = Time.time;

        // Stop movement but retain rotation
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        // Play squeak sound on click
        SoundFXManager.instance.PlaySoundByName("Squeak", transform);
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePos);
        }
    }

    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;

        float dragDuration = Time.time - dragStartTime;
        Vector2 dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        rb.gravityScale = 0f;

        if (dragDuration <= 0.1f)
        {
            ApplyRandomMotion();
        }
        else
        {
            Vector2 throwDirection = (dragEndPos - dragStartPos).normalized;
            float throwStrength = Vector2.Distance(dragEndPos, dragStartPos) / dragDuration;

            rb.velocity = throwDirection * throwStrength;
        }
    }

    private void ApplyRandomMotion()
    {
        float spinSpeed = Random.Range(minAngularVelocity, maxAngularVelocity);
        float spinDirection = Random.value < 0.5f ? -1f : 1f;
        rb.angularVelocity = spinSpeed * spinDirection;

        Vector2 moveDirection = Random.insideUnitCircle.normalized;
        float moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        rb.velocity = moveDirection * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundFXManager.instance.PlaySoundByName("HamsterHit", transform);
    }
}
