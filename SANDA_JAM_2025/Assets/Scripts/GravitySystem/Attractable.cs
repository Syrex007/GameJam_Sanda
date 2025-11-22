using UnityEngine;
using DG.Tweening;

public class Attractable : MonoBehaviour
{
    [SerializeField] private bool rotateToCenter = true;
    [SerializeField] private Attractor currentAttractor;
    [SerializeField] private float gravityStrength = 100;

    [Header("Freeze Settings")]
    public float slowDuration = 0.1f;     // Tiempo de desaceleración
    public float freezeDelay = 0.01f;      // Delay antes del freeze real

    Transform m_transform;
    Collider2D m_collider;
    Rigidbody2D m_rigdibody;

    Vector3 originalScale;
    bool isFrozen;

    private void Start()
    {
        m_transform = GetComponent<Transform>();
        m_collider = GetComponent<Collider2D>();
        m_rigdibody = GetComponent<Rigidbody2D>();

        originalScale = m_transform.localScale;
    }

    private void Update()
    {
        if (currentAttractor != null)
        {
            if (!currentAttractor.AttractedObjects.Contains(m_collider))
            {
                currentAttractor = null;
                isFrozen = false;
                return;
            }

            m_rigdibody.gravityScale = 0;

            if (currentAttractor.gravity == 0)
            {
                FreezeObject();
                return;
            }

            if (rotateToCenter)
                RotateToCenter();
        }
    }

    public void Attract(Attractor attractorObj)
    {
        if (currentAttractor != null && currentAttractor.gravity == 0)
            return;

        if (currentAttractor == null || attractorObj.gravity == 0)
            currentAttractor = attractorObj;

        if (attractorObj.gravity == 0)
        {
            FreezeObject();
            return;
        }

        Vector2 attractionDir = ((Vector2)attractorObj.attractorTransform.position - m_rigdibody.position).normalized;

        m_rigdibody.AddForce(attractionDir * -attractorObj.gravity * gravityStrength * Time.fixedDeltaTime);
    }

void FreezeObject()
{
    if (isFrozen) return;

    isFrozen = true;

    DOTween.Kill(m_rigdibody);

    Vector2 initialVel = m_rigdibody.velocity;

    // Desaceleración más notoria
    DOTween.To(() => initialVel, v =>
    {
        initialVel = v;
        m_rigdibody.velocity = v;
    },
    Vector2.zero,
    slowDuration)
    .SetEase(Ease.OutCubic)   // Más brusco al final
    .OnComplete(() =>
    {
        // Mantiene el movimiento pero lo obliga a apagarse totalmente
        m_rigdibody.velocity = Vector2.zero;
        m_rigdibody.angularVelocity = 0f;
        m_rigdibody.gravityScale = 0;
    });

    // Hacer el visual MÁS fuerte
    FreezeVisual();
}


    System.Collections.IEnumerator FinalFreeze()
    {
        yield return new WaitForSeconds(freezeDelay);

        m_rigdibody.velocity = Vector2.zero;
        m_rigdibody.angularVelocity = 0f;

        // Freeze real
        m_rigdibody.bodyType = RigidbodyType2D.Static;
    }

void FreezeVisual()
{
    m_transform.DOKill();
    m_transform.localScale = originalScale;

    m_transform.DOShakeScale(
        0.05f,   // más largo
        0.45f,   // más fuerte
        18,      // más vibración
        100f,    // más snappy
        false
    )
    .OnComplete(() =>
    {
        m_transform.localScale = originalScale;
    });
}


    void RotateToCenter()
    {
        if (currentAttractor != null && currentAttractor.gravity != 0)
        {
            Vector2 distanceVector = (Vector2)currentAttractor.attractorTransform.position - (Vector2)m_transform.position;
            float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
            m_transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        }
    }
}
