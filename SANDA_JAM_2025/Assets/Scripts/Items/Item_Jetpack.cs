using UnityEngine;


public class Item_Jetpack : MonoBehaviour
{
    [SerializeField] private float forceAmount = 5f;
    [SerializeField] private Rigidbody2D rb;

    void Awake()
    {
       
    }

    public void ApplyJetpackForce()
    {
        // Apply force in the direction the object is facing
        rb.AddForce(transform.up * forceAmount, ForceMode2D.Force);
    }
}
