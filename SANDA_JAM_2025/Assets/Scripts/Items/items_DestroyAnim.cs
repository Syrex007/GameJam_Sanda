using UnityEngine;
using DG.Tweening;

public class ScaleDownWithBounce : MonoBehaviour
{
    [SerializeField] private float duration = 1f;

    void Start()
    {
        // Start the bounce scale down
        transform.DOScale(Vector3.zero, duration)
                 .SetEase(Ease.InBack); // Bouncy easing
    }
}
