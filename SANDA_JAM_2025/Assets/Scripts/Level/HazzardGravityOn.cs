using UnityEngine;

public class HazzardGravityOn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    [SerializeField] private float minAngularSpeed = -180f;
    [SerializeField] private float maxAngularSpeed = 180f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Apply random initial rotation
        float randomAngle = Random.Range(0f, 360f);
        rb.rotation = randomAngle;

        // Apply random angular velocity
        rb.angularVelocity = Random.Range(minAngularSpeed, maxAngularSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
