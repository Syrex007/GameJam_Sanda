using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OrbitHazzard : MonoBehaviour
{
    public Transform centerPoint;
    public float orbitRadius = 3f;
    public float orbitSpeed = 90f; // Degrees per second

    private float currentAngle = 0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        currentAngle += orbitSpeed * Time.fixedDeltaTime;
        if (currentAngle > 360f) currentAngle -= 360f;

        Vector2 offset = new Vector2(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad),
            Mathf.Sin(currentAngle * Mathf.Deg2Rad)
        ) * orbitRadius;

        Vector2 targetPosition = (Vector2)centerPoint.position + offset;

        rb.MovePosition(targetPosition);
    }
}
