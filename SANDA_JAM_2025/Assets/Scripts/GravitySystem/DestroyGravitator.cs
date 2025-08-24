using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DestroyGravitator : MonoBehaviour
{
    [SerializeField] private Slider gravitatorLifeTime;
    [SerializeField] private GameObject objectToSpawnOnDestroy; // Prefab to instantiate

    private Coroutine destroyCoroutine;

    public void DestroySelf(float time)
    {
        gravitatorLifeTime.maxValue = time;
        gravitatorLifeTime.value = time;

        if (destroyCoroutine != null)
            StopCoroutine(destroyCoroutine);

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

        // Instantiate the new object at this position and rotation
        if (objectToSpawnOnDestroy != null)
        {
            Instantiate(objectToSpawnOnDestroy, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}
