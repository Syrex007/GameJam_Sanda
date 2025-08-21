using System.Collections;
using UnityEngine;

public class DestroyGravitator : MonoBehaviour
{

    private Coroutine destroyCoroutine;

    public void DestroySelf(float time)
    {
        if (destroyCoroutine != null)
            StopCoroutine(destroyCoroutine);

        destroyCoroutine = StartCoroutine(DestroyAfterTime(time));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Me voy a destruir");
        Destroy(gameObject);
    }
}
