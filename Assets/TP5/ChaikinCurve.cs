using System.Collections.Generic;
using UnityEngine;

public class ChaikinCurve : MonoBehaviour
{
    public List<Vector3> points; // Les points initiaux du polygone
    public int iterations = 3; // Nombre d'itérations de l'algorithme

    void OnDrawGizmos()
    {
        if (points == null || points.Count < 2)
            return;

        Gizmos.color = Color.yellow;

        // Créer une liste temporaire pour contenir les points lissés
        List<Vector3> smoothedPoints = new List<Vector3>(points);

        // Appliquer l'algorithme de Chaikin pour un nombre d'itérations défini
        for (int i = 0; i < iterations; i++)
        {
            smoothedPoints = ChaikinSubdivision(smoothedPoints);
        }

        // Dessiner les lignes entre les points lissés
        for (int i = 0; i < smoothedPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(smoothedPoints[i], smoothedPoints[i + 1]);
        }

        // Relier le dernier point au premier pour fermer la forme si nécessaire
        if (smoothedPoints.Count > 1)
        {
            Gizmos.DrawLine(smoothedPoints[smoothedPoints.Count - 1], smoothedPoints[0]);
        }
    }

    // Fonction de subdivision Chaikin
    List<Vector3> ChaikinSubdivision(List<Vector3> points)
    {
        List<Vector3> newPoints = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = points[i];
            Vector3 p1 = points[i + 1];

            // Calculer les nouveaux points en subdivisant le segment
            Vector3 q = Vector3.Lerp(p0, p1, 0.25f);
            Vector3 r = Vector3.Lerp(p0, p1, 0.75f);

            newPoints.Add(q);
            newPoints.Add(r);
        }

        return newPoints;
    }
}
