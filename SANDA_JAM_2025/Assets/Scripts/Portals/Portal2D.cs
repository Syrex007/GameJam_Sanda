using UnityEngine;
using System.Collections.Generic;

public class Portal2D : MonoBehaviour
{
    public enum ExitOrientation
    {
        KeepVelocity,      // Mantiene la velocidad tal cual
        RedirectToUp,      // Redirige hacia el up del portal destino
        RedirectToRight,   // Redirige hacia el right del portal destino
        InvertVelocity     // Invierte la velocidad
    }

    [Header("Portal Settings")]
    public Portal2D pairedPortal;
    public Transform exitPoint;
    public ExitOrientation orientationMode = ExitOrientation.KeepVelocity;

    [Header("Layer Filter")]
    public LayerMask attractableLayer;

    [Header("Exit Settings")]
    public float exitOffset = 0.5f;       // Qué tan afuera del portal sale
    public float teleportCooldown = 0.15f; // Evita loops

    private HashSet<Collider2D> cooldownList = new HashSet<Collider2D>();


    // ---------------------------------------------------
    // ENTRADA AL PORTAL
    // ---------------------------------------------------
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Solo objetos attractable
        if ((attractableLayer.value & (1 << col.gameObject.layer)) == 0)
            return;

        Rigidbody2D rb = col.attachedRigidbody;
        if (rb == null) return;

        if (pairedPortal == null || pairedPortal.exitPoint == null)
            return;

        // Si está en cooldown → ignorar
        if (cooldownList.Contains(col))
            return;

        Teleport(col, rb);
    }


    // ---------------------------------------------------
    // TELETRANSPORTE
    // ---------------------------------------------------
    private void Teleport(Collider2D col, Rigidbody2D rb)
    {
        Vector2 storedVelocity = rb.velocity;

        // Aplicar orientación
        storedVelocity = ApplyOrientation(storedVelocity, pairedPortal.exitPoint);

        // Posición base de salida
        Vector2 exitBasePos = (Vector2)pairedPortal.exitPoint.position +
                              (Vector2)pairedPortal.exitPoint.up * exitOffset;

        // Evitar superposición con búsqueda circular
        Vector2 finalExitPos = pairedPortal.FindFreeExitPosition(exitBasePos);

        // Teleport REAL
        rb.position = finalExitPos;
        rb.velocity = storedVelocity;

        // Cooldown en ambos portales
        cooldownList.Add(col);
        pairedPortal.cooldownList.Add(col);

        StartCoroutine(RemoveCooldown(col, teleportCooldown));
        pairedPortal.StartCoroutine(pairedPortal.RemoveCooldown(col, teleportCooldown));
    }


    // ---------------------------------------------------
    // ORIENTAR VELOCIDAD
    // ---------------------------------------------------
    private Vector2 ApplyOrientation(Vector2 vel, Transform exit)
    {
        switch (orientationMode)
        {
            case ExitOrientation.KeepVelocity:
                return vel;

            case ExitOrientation.RedirectToUp:
                return exit.up.normalized * vel.magnitude;

            case ExitOrientation.RedirectToRight:
                return exit.right.normalized * vel.magnitude;

            case ExitOrientation.InvertVelocity:
                return -vel;

            default:
                return vel;
        }
    }


    // ---------------------------------------------------
    // POSICIÓN LIBRE EN SALIDA
    // ---------------------------------------------------
    public Vector2 FindFreeExitPosition(Vector2 exitBasePos)
    {
        float radius = 0.56f;
        int checks = 12;
        float distance = 0.45f;

        // Primero probar salida directa
        if (!Physics2D.OverlapCircle(exitBasePos, radius, attractableLayer))
            return exitBasePos;

        // Buscar alrededor del portal destino
        for (int i = 0; i < checks; i++)
        {
            float angle = (360f / checks) * i;
            Vector2 offset = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ) * distance;

            Vector2 testPos = exitBasePos + offset;

            if (!Physics2D.OverlapCircle(testPos, radius, attractableLayer))
                return testPos;
        }

        // Si no encuentra nada → empujar arriba del portal
        return exitBasePos + (Vector2)exitPoint.up * 0.45f;
    }


    // ---------------------------------------------------
    // COOLDOWN
    // ---------------------------------------------------
    private IEnumerator<System.Object> RemoveCooldown(Collider2D col, float time)
    {
        yield return new WaitForSeconds(time);
        cooldownList.Remove(col);
    }


    // ---------------------------------------------------
    // GIZMOS
    // ---------------------------------------------------
    private void OnDrawGizmos()
    {
        if (exitPoint == null) return;

        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(exitPoint.position, 0.12f);
        Gizmos.DrawLine(exitPoint.position,
                        exitPoint.position + exitPoint.up * 0.6f);

        DrawArrow(exitPoint.position, exitPoint.up, 0.6f);
    }

    private void DrawArrow(Vector3 pos, Vector3 dir, float length)
    {
        Vector3 end = pos + dir * length;
        Gizmos.DrawLine(pos, end);

        Vector3 right = Quaternion.Euler(0, 0, 150) * dir;
        Vector3 left  = Quaternion.Euler(0, 0, -150) * dir;

        Gizmos.DrawLine(end, end + right * 0.2f);
        Gizmos.DrawLine(end, end + left  * 0.2f);
    }
}
