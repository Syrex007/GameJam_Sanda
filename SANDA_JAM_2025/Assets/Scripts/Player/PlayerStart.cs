using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerStart : MonoBehaviour
{
    [Header("Configuración Física")]
    [SerializeField] private float minAngularSpeed = -180f;
    [SerializeField] private float maxAngularSpeed = 180f;

    [Header("Animaciones")]
    [SerializeField] private Animator hamsterAnimator;
    [SerializeField] private string idleAnimation = "Idle";
    [SerializeField] private string rightUpAnimation = "RightUp";
    [SerializeField] private string rightDownAnimation = "RightDown";
    [SerializeField] private string leftUpAnimation = "LeftUp";
    [SerializeField] private string leftDownAnimation = "LeftDown";

    [Header("Sensibilidad del movimiento")]
    [SerializeField] private float movementThreshold = 1.5f; // Ajusta este valor para controlar cuándo vuelve a Idle

    [Header("Win Config")]
    [SerializeField] private GameObject winZone;

    private Rigidbody2D rb;
    private bool goalAnimationStarted = false;
    private string currentAnimation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Rotación inicial aleatoria
        float randomAngle = Random.Range(0f, 360f);
        rb.rotation = randomAngle;

        // Velocidad angular inicial aleatoria
        rb.angularVelocity = Random.Range(minAngularSpeed, maxAngularSpeed);

        // Iniciamos en Idle
        PlayAnimation(idleAnimation);
    }

    void Update()
    {
        // Si alcanzamos la meta, ejecutamos la animación de victoria
        if (GameManager.instance.goalReached && !goalAnimationStarted)
        {
            goalAnimationStarted = true;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;

            transform.DOMove(winZone.transform.position, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOScale(Vector3.zero, 1f).SetEase(Ease.Linear);
                transform.DORotate(new Vector3(0, 0, 1000f), 1f, RotateMode.FastBeyond360)
                         .SetEase(Ease.Linear)
                         .SetLoops(-1, LoopType.Incremental);
            });

            return;
        }

        HandleMovementAnimations();

        // Si usas velocidad angular como parámetro en el Animator
        hamsterAnimator.SetFloat("RotationSpeed", Mathf.Abs(rb.angularVelocity));
    }

    /// <summary>
    /// Maneja las animaciones del hamster dependiendo de la velocidad, dirección y rotación.
    /// </summary>
    private void HandleMovementAnimations()
    {
        Vector2 velocity = rb.linearVelocity;

        // Si la velocidad es menor que el umbral, reproducir Idle
        if (velocity.sqrMagnitude < movementThreshold * movementThreshold)
        {
            PlayAnimation(idleAnimation);
            return;
        }

        // Convertimos la velocidad global a local según la rotación del hamster
        Vector2 localVelocity = transform.InverseTransformDirection(velocity);

        // Determinar cuadrante basado en la dirección local del movimiento
        if (localVelocity.x >= 0 && localVelocity.y >= 0)
        {
            PlayAnimation(rightUpAnimation); // Derecha + Arriba (local)
        }
        else if (localVelocity.x >= 0 && localVelocity.y < 0)
        {
            PlayAnimation(rightDownAnimation); // Derecha + Abajo (local)
        }
        else if (localVelocity.x < 0 && localVelocity.y >= 0)
        {
            PlayAnimation(leftUpAnimation); // Izquierda + Arriba (local)
        }
        else
        {
            PlayAnimation(leftDownAnimation); // Izquierda + Abajo (local)
        }
    }

    /// <summary>
    /// Cambia la animación actual solo si es diferente.
    /// </summary>
    private void PlayAnimation(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        hamsterAnimator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sonido al colisionar
        SoundFXManager.instance.PlaySoundByName("Bounce", transform);
    }
}
