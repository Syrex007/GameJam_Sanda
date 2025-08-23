using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower2D : MonoBehaviour
{
    public enum MovementMode
    {
        OneWay,   // Va del primer nodo al último y termina.
        Loop,     // Vuelve del último nodo al primero y repite infinito.
        PingPong  // Va y regresa entre los nodos.
    }

    [Header("Configuración del recorrido")]
    public List<Transform> pathNodes = new List<Transform>();

    public float moveSpeed = 3f;

    public float pausaPorNodo = 0f;

    public MovementMode movementMode = MovementMode.OneWay;

    [Header("Gizmos")]
    public Color gizmoColor = Color.cyan;
    public Color nodoColor = Color.red;
    public float tamañoNodo = 0.15f;

    private int currentNodeIndex = 0;
    private bool isMoving = true;
    private bool goingForward = true;
    private bool isPaused = false;

    private void Start()
    {
        if (pathNodes.Count > 0)
            transform.position = pathNodes[0].position;
    }

    private void Update()
    {
        if (!isMoving || pathNodes.Count < 2 || isPaused)
            return;

        Transform objetivo = pathNodes[currentNodeIndex];
        transform.position = Vector2.MoveTowards(transform.position, objetivo.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, objetivo.position) <= 0.05f)
        {
            StartCoroutine(HandleNodeReached());
        }
    }

    private IEnumerator HandleNodeReached()
    {
        isPaused = true;

        // Aplica pausa si está configurada
        if (pausaPorNodo > 0)
            yield return new WaitForSeconds(pausaPorNodo);

        switch (movementMode)
        {
            // Llega al final y se detiene --------
            case MovementMode.OneWay:
                currentNodeIndex++;
                if (currentNodeIndex >= pathNodes.Count)
                {
                    isMoving = false; // Detener el movimiento al final
                    yield break;
                }
                break;

            // Del último nodo al primero --------
            case MovementMode.Loop:
                currentNodeIndex++;
                if (currentNodeIndex >= pathNodes.Count)
                {
                    currentNodeIndex = 0; // Reinicia al primer nodo
                }
                break;

            // Va y regresa
            case MovementMode.PingPong:
                if (goingForward)
                {
                    currentNodeIndex++;
                    if (currentNodeIndex >= pathNodes.Count)
                    {
                        currentNodeIndex = pathNodes.Count - 2;
                        goingForward = false;
                    }
                }
                else
                {
                    currentNodeIndex--;
                    if (currentNodeIndex < 0)
                    {
                        currentNodeIndex = 1;
                        goingForward = true;
                    }
                }
                break;
        }

        isPaused = false;
    }

    private void OnDrawGizmos()
    {
        if (pathNodes == null || pathNodes.Count == 0)
            return;

        Gizmos.color = gizmoColor;

        for (int i = 0; i < pathNodes.Count; i++)
        {
            if (pathNodes[i] == null)
                continue;

            // Dibuja nodos
            Gizmos.color = nodoColor;
            Gizmos.DrawSphere(pathNodes[i].position, tamañoNodo);

            // Dibuja líneas entre nodos consecutivos
            if (i < pathNodes.Count - 1)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawLine(pathNodes[i].position, pathNodes[i + 1].position);
            }
        }

        // Si el modo es LOOP → une primer y último nodo
        if (movementMode == MovementMode.Loop && pathNodes.Count > 1)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawLine(pathNodes[0].position, pathNodes[pathNodes.Count - 1].position);
        }
    }
}
