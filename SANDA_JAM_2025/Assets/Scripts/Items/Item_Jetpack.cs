using UnityEngine;

public class Item_Jetpack : MonoBehaviour
{
    [SerializeField] private float forceAmount = 5f;
    [SerializeField] private int particleRate = 50;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem jetpackParticles;

    private ParticleSystem.EmissionModule emission;

    void Awake()
    {
        // Cache the emission module for efficiency
        if (jetpackParticles != null)
            emission = jetpackParticles.emission;
    }

    public void ApplyJetpackForce()
    {
        // Apply upward force in the direction the object is facing
        rb.AddForce(transform.up * forceAmount, ForceMode2D.Force);

        // Enable particle emission
        if (jetpackParticles != null)
            emission.rateOverTime = particleRate;
    }

    public void StopJetpackForce()
    {
        // Stop particle emission
        if (jetpackParticles != null)
            emission.rateOverTime = 0f;
    }
}
