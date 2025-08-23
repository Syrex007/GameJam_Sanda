using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DestroyGravitator : MonoBehaviour
{

    private Coroutine destroyCoroutine;
    [SerializeField] private Slider gravitatorLifeTime;

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
        Destroy(gameObject);
    }
}
