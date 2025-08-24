using UnityEngine;

public class Item_Jetpack : MonoBehaviour
{
    [SerializeField] private float forceAmount = 5f;
    [SerializeField] private int particleRate = 50;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem jetpackParticles;

    private ParticleSystem.EmissionModule emission;
    private bool soundPlaying = false;
    private string jetpackSoundName = "Extinguisher"; // Make sure this matches the clip name

    void Awake()
    {
        if (jetpackParticles != null)
            emission = jetpackParticles.emission;
    }

    public void ApplyJetpackForce()
    {
        // Apply upward force
        rb.AddForce(transform.up * forceAmount, ForceMode2D.Force);

        // Enable particle emission
        if (jetpackParticles != null)
            emission.rateOverTime = particleRate;

        // Play looping sound if not already playing
        if (!soundPlaying)
        {
            SoundFXManager.instance.PlaySoundByName(jetpackSoundName, transform, volume: 1f, pitch: 1f, loop: true);
            soundPlaying = true;
        }
    }

    public void StopJetpackForce()
    {
        // Stop particle emission
        if (jetpackParticles != null)
            emission.rateOverTime = 0f;

        // Stop looping sound
        if (soundPlaying)
        {
            SoundFXManager.instance.StopSoundByName(jetpackSoundName);
            soundPlaying = false;
        }
    }
}
