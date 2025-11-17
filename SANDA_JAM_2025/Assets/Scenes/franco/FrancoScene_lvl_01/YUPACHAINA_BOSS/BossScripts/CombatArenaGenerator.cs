using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class CombatArenaGenerator : MonoBehaviour
{
    [Header("Configuración de la Arena")]
    public Transform arenaCenter;
    [Range(3, 12)]
    public int numberOfSides = 8;
    public float radius = 10f;
    public bool includeCenterAnchor = true;

    [HideInInspector]
    public List<Vector3> anchorPositions = new List<Vector3>();

    private void OnValidate()
    {
        GenerateAnchorPoints();
    }

    private void Start()
    {
        if (arenaCenter == null)
        {
            GameObject centerObj = new GameObject("ArenaCenter");
            arenaCenter = centerObj.transform;
            arenaCenter.position = Vector3.zero;
        }

        GenerateAnchorPoints();
    }

    public void GenerateAnchorPoints()
    {
        anchorPositions.Clear();
        if (arenaCenter == null)
            return;

        for (int i = 0; i < numberOfSides; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfSides;
            Vector3 point = arenaCenter.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            anchorPositions.Add(point);
        }

        if (includeCenterAnchor)
            anchorPositions.Add(arenaCenter.position);
    }

    private void OnDrawGizmos()
    {
        if (arenaCenter == null) return;

        // Dibujar el polígono base
        Gizmos.color = Color.cyan;
        Vector3 prevPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;

        for (int i = 0; i < numberOfSides; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfSides;
            Vector3 point = arenaCenter.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            if (i == 0)
                firstPoint = point;
            else
                Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
        Gizmos.DrawLine(prevPoint, firstPoint);

        // Vértices (anchors)
        Gizmos.color = Color.yellow;
        foreach (var p in anchorPositions)
            Gizmos.DrawSphere(p, 0.25f);
    }
}
