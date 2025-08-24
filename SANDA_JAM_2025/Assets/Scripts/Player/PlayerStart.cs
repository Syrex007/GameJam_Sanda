using UnityEngine;
using DG.Tweening; // DOTween namespace

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStart : MonoBehaviour
{
    [SerializeField] private float minAngularSpeed = -180f;
    [SerializeField] private float maxAngularSpeed = 180f;

    private Rigidbody2D rb;
    private bool goalAnimationStarted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Apply random initial rotation
        float randomAngle = Random.Range(0f, 360f);
        rb.rotation = randomAngle;

        // Apply random angular velocity
        rb.angularVelocity = Random.Range(minAngularSpeed, maxAngularSpeed);
    }

    private void Update()
    {
        if (GameManager.instance.goalReached && !goalAnimationStarted)
        {
            goalAnimationStarted = true;

            
            rb.angularVelocity = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;

            // Begin scale and rotation tween
            transform.DOScale(Vector3.zero, 1f).SetEase(Ease.Linear);
            transform.DORotate(new Vector3(0, 0, 1000f), 1f, RotateMode.FastBeyond360)
                     .SetEase(Ease.Linear)
                     .SetLoops(-1, LoopType.Incremental); 
        }
    }
}
