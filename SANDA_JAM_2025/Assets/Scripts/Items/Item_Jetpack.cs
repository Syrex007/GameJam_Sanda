using UnityEngine;
using UnityEngine.UI;

public class Item_Jetpack : MonoBehaviour
{
    [SerializeField] private float forceAmount = 5f;
    [SerializeField] private int particleRate = 50;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Rigidbody2D rbSelf;
    [SerializeField] private ParticleSystem jetpackParticles;
    [SerializeField] private Slider fuelSlider; // Link this in Inspector
    [SerializeField] private string jetpackSoundName = "Extinguisher";
    [SerializeField] private string detachSoundName = "Detach"; // Sound to play when detaching

    private ParticleSystem.EmissionModule emission;
    private bool soundPlaying = false;
    private bool hasDetached = false;

    void Awake()
    {
        if (jetpackParticles != null)
            emission = jetpackParticles.emission;
    }

    void Update()
    {
        if (fuelSlider != null && !hasDetached && fuelSlider.value <= 0)
        {
            DetachJetpack();
        }
    }

    public void ApplyJetpackForce()
    {
        rb.AddForce(transform.up * forceAmount, ForceMode2D.Force);

        if (jetpackParticles != null)
            emission.rateOverTime = particleRate;

        if (!soundPlaying)
        {
            SoundFXManager.instance.PlaySoundByName(jetpackSoundName, transform, volume: 1f, pitch: 1f, loop: true);
            soundPlaying = true;
        }
    }

    public void StopJetpackForce()
    {
        if (jetpackParticles != null)
            emission.rateOverTime = 0f;

        if (soundPlaying)
        {
            SoundFXManager.instance.StopSoundByName(jetpackSoundName);
            soundPlaying = false;
        }
    }

    private void DetachJetpack()
    {
        hasDetached = true;

        // Detach from parent
        transform.parent = null;

        // Change Rigidbody type to Dynamic
        rbSelf.bodyType = RigidbodyType2D.Dynamic;

        rbSelf.gravityScale = 0f; // Enable gravity
        // Apply random angular velocity
        rbSelf.angularVelocity = Random.Range(-360f, 360f);

        // Optionally apply a small force to "drop" it
        rbSelf.AddForce(Random.insideUnitCircle.normalized * 2f, ForceMode2D.Impulse);

        // Stop jetpack loop sound
        if (soundPlaying)
        {
            SoundFXManager.instance.StopSoundByName(jetpackSoundName);
            soundPlaying = false;
        }

        // Play detach sound
        SoundFXManager.instance.PlaySoundByName(detachSoundName, transform);
    }
}
