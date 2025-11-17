using UnityEngine;
using System.Collections.Generic;

public class Portal2D : MonoBehaviour
{
    [Header("Portal Settings")]
    public Portal2D pairedPortal;

    [Tooltip("Punto exacto donde aparecerá el objeto al salir del portal.")]
    public Transform exitPoint;

    [Header("Layer Filter")]
    public LayerMask attractableLayer;

    public float exitOffset = 0.5f;
    public float teleportCooldown = 0.15f;

    private HashSet<Collider2D> cooldownList = new HashSet<Collider2D>();


    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((attractableLayer.value & (1 << col.gameObject.layer)) == 0)
            return;

        Rigidbody2D rb = col.attachedRigidbody;
        if (rb == null) return;

        if (pairedPortal == null || pairedPortal.exitPoint == null) return;

        if (cooldownList.Contains(col)) return;

        Teleport(col, rb);
    }


    private void Teleport(Collider2D col, Rigidbody2D rb)
    {
        Vector2 storedVelocity = rb.velocity;

        Vector3 exitPos = pairedPortal.exitPoint.position;
        exitPos += pairedPortal.exitPoint.up * exitOffset;

        rb.position = exitPos;
        rb.velocity = storedVelocity;

        cooldownList.Add(col);
        pairedPortal.cooldownList.Add(col);

        StartCoroutine(RemoveCooldown(col, teleportCooldown));
        pairedPortal.StartCoroutine(pairedPortal.RemoveCooldown(col, teleportCooldown));
    }


    private IEnumerator<System.Object> RemoveCooldown(Collider2D col, float t)
    {
        yield return new WaitForSeconds(t);
        cooldownList.Remove(col);
    }


    // ---------- GIZMOS PARA ARRÁSTRAR EXIT POINT ----------
    private void OnDrawGizmos()
    {
        if (exitPoint == null) return;

        Gizmos.color = Color.cyan;

        // esfera del exit point
        Gizmos.DrawSphere(exitPoint.position, 0.12f);

        // línea indicando dirección de salida
        Gizmos.DrawLine(exitPoint.position, exitPoint.position + exitPoint.up * 0.6f);

        // flecha visual
        DrawArrow(exitPoint.position, exitPoint.up, 0.6f);
    }

    private void DrawArrow(Vector3 pos, Vector3 dir, float length)
    {
        Vector3 end = pos + dir * length;
        Gizmos.DrawLine(pos, end);

        Vector3 right = Quaternion.Euler(0, 0, 150) * dir;
        Vector3 left = Quaternion.Euler(0, 0, -150) * dir;

        Gizmos.DrawLine(end, end + right * 0.2f);
        Gizmos.DrawLine(end, end + left * 0.2f);
    }
}
