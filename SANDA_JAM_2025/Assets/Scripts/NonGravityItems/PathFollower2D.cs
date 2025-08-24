using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Para que no caiga si hay físicas 2D activas
        rb.freezeRotation = true; // No queremos que rote al moverse
    }

    private void Start()
    {
        if (pathNodes.Count > 0)
            rb.position = pathNodes[0].position; // Ajustamos la posición inicial del rigidbody
    }

    private void FixedUpdate()
    {
        if (!isMoving || pathNodes.Count < 2 || isPaused)
            return;

        Transform objetivo = pathNodes[currentNodeIndex];

        // Calculamos la nueva posición usando MovePosition
        Vector2 nuevaPosicion = Vector2.MoveTowards(rb.position, objetivo.position, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(nuevaPosicion);

        // Si llegamos al nodo, procesamos la pausa o el siguiente nodo
        if (Vector2.Distance(rb.position, objetivo.position) <= 0.05f)
        {
            StartCoroutine(HandleNodeReached());
        }
    }

    private IEnumerator HandleNodeReached()
    {
        isPaused = true;

        // Si hay pausa configurada, esperamos en el nodo
        if (pausaPorNodo > 0)
            yield return new WaitForSeconds(pausaPorNodo);

        switch (movementMode)
        {
            case MovementMode.OneWay:
                currentNodeIndex++;
                if (currentNodeIndex >= pathNodes.Count)
                {
                    isMoving = false; // Termina el recorrido
                    yield break;
                }
                break;

            case MovementMode.Loop:
                currentNodeIndex++;
                if (currentNodeIndex >= pathNodes.Count)
                    currentNodeIndex = 0;
                break;

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
