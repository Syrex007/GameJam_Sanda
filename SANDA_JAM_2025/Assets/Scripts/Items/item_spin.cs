using UnityEngine;
using DG.Tweening;

public class item_spin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f; // degrees per second

    void Start()
    {
        // Calculate duration to complete 360° based on speed
        float duration = 360f / rotationSpeed;

        // Spin infinitely
        transform.DORotate(
            new Vector3(0, 0, 360f),
            duration,
            RotateMode.FastBeyond360
        )
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental);
    }
}
