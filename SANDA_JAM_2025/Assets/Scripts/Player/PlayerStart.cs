using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStart : MonoBehaviour
{
    [SerializeField] private float minAngularSpeed = -180f;
    [SerializeField] private float maxAngularSpeed = 180f;
    [SerializeField] private GameObject winZone;
    [SerializeField] private Animator hamsterAnimator;
    [SerializeField] private string collisionSoundName = "HamsterHit"; // Set this to the clip name in SoundFXManager

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

            // Stop physics simulation before tweening
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;

            // Move to win zone with DOTween, then scale down and rotate
            transform.DOMove(winZone.transform.position, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOScale(Vector3.zero, 1f).SetEase(Ease.Linear);
                transform.DORotate(new Vector3(0, 0, 1000f), 1f, RotateMode.FastBeyond360)
                         .SetEase(Ease.Linear)
                         .SetLoops(-1, LoopType.Incremental);
            });
        }

        hamsterAnimator.SetFloat("RotationSpeed", Mathf.Abs(rb.angularVelocity));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Play sound when colliding with any object
        SoundFXManager.instance.PlaySoundByName(collisionSoundName, transform);
    }
}
