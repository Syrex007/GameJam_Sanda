using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DestroyGravitator : MonoBehaviour
{
    [SerializeField] private Slider gravitatorLifeTime;
    [SerializeField] private GameObject objectToSpawnOnDestroy;
    [SerializeField] private string loopSoundName = "GravitatorLoop"; // Name of the sound

    private Coroutine destroyCoroutine;
    private bool soundPlaying = false;

    public void DestroySelf(float time)
    {
        gravitatorLifeTime.maxValue = time;
        gravitatorLifeTime.value = time;

        if (destroyCoroutine != null)
            StopCoroutine(destroyCoroutine);

        // Start looping sound
        if (!soundPlaying)
        {
            SoundFXManager.instance.PlaySoundByName(loopSoundName, transform, volume: 0.2f, pitch: 0.4f, loop: true);
            soundPlaying = true;
        }

        destroyCoroutine = StartCoroutine(DestroyAfterTime(time));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            gravitatorLifeTime.value = time - elapsedTime;
            yield return null;
        }

        gravitatorLifeTime.value = 0;
        Debug.Log("Me voy a destruir");

        // Stop looping sound
        if (soundPlaying)
        {
            SoundFXManager.instance.StopSoundByName(loopSoundName);
            soundPlaying = false;
        }

        // Spawn object on destroy
        if (objectToSpawnOnDestroy != null)
        {
            Instantiate(objectToSpawnOnDestroy, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
