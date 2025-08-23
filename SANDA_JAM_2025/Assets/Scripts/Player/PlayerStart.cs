using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStart : MonoBehaviour
{
    [SerializeField] private float minAngularSpeed = -180f;
    [SerializeField] private float maxAngularSpeed = 180f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Apply random initial rotation
        float randomAngle = Random.Range(0f, 360f);
        rb.rotation = randomAngle;

        // Apply random angular velocity (physics-based rotation)
        rb.angularVelocity = Random.Range(minAngularSpeed, maxAngularSpeed);
    }
}
