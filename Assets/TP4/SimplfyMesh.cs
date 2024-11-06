using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplfyMesh : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [Tooltip("Ajuster la taille des cellules")]
    public float cellSize = .1f;
    [Tooltip("Stocke le maillage de départ pour permettre un reset.")]
    public Mesh start_mesh;

    public void ResetMesh()
    {
        GetComponent<MeshFilter>().mesh = start_mesh;
    }

    // Fonction principale pour simplifier le maillage.
    public void SimplifyMesh()
    {
        // Récupère le maillage actuel.
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Stocke les sommets et triangles d'origine.
        Vector3[] originalVertices = mesh.vertices;
        int[] originalTriangles = mesh.triangles;

        // On appelle la fonction pour fusionner les sommets proches en fonction de la taille des cellules.
        MergeVertices(originalVertices, cellSize, out Vector3[] newVertices, out int[] map);

        // On ajuste les triangles pour pointer vers les nouveaux sommets fusionnés.
        int[] newTriangles = UpdateTriangles(originalTriangles, map);

        // Applique les modifications au maillage.
        mesh.Clear();
        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // Fonction qui calcule la position de la cellule pour un point donné.
    Vector3 CellPosition(Vector3 point, float cellSize)
    {
        // Arrondit les coordonnées du point pour obtenir la cellule correspondante.
        return new Vector3(
            Mathf.Floor(point.x / cellSize) * cellSize,
            Mathf.Floor(point.y / cellSize) * cellSize,
            Mathf.Floor(point.z / cellSize) * cellSize
        );
    }

    // Fonction pour fusionner les sommets proches en un seul sommet.
    void MergeVertices(Vector3[] vertices, float cellSize, out Vector3[] newVertices, out int[] map)
    {
        // Dictionnaire pour stocker les indices des sommets de chaque cellule.
        Dictionary<Vector3, List<int>> cellToVertices = new Dictionary<Vector3, List<int>>();
        map = new int[vertices.Length];

        // Assigne chaque sommet à une cellule.
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 cellPos = CellPosition(vertices[i], cellSize);

            // Si la cellule n'existe pas, on la crée.
            if (!cellToVertices.ContainsKey(cellPos))
            {
                cellToVertices[cellPos] = new List<int>();
            }
            cellToVertices[cellPos].Add(i);
        }

        // Liste pour stocker les nouveaux sommets après fusion.
        List<Vector3> mergedVertices = new List<Vector3>();
        int newIndex = 0;

        // Parcourt chaque cellule et fusionne les sommets en calculant la position moyenne.
        foreach (var kvp in cellToVertices)
        {
            Vector3 averagePos = Vector3.zero;
            foreach (int vertIndex in kvp.Value)
            {
                averagePos += vertices[vertIndex];
            }
            averagePos /= kvp.Value.Count;

            mergedVertices.Add(averagePos);

            // Associe chaque ancien sommet à un nouvel index.
            foreach (int vertIndex in kvp.Value)
            {
                map[vertIndex] = newIndex;
            }

            newIndex++;
        }

        // Convertit la liste de sommets fusionnés en un tableau.
        newVertices = mergedVertices.ToArray();
    }

    // Fonction pour mettre à jour les triangles en fonction des nouveaux sommets fusionnés.
    int[] UpdateTriangles(int[] triangles, int[] map)
    {
        // Tableau pour stocker les nouveaux triangles.
        int[] newTriangles = new int[triangles.Length];

        // Met à jour chaque index de triangle pour pointer vers le nouveau sommet.
        for (int i = 0; i < triangles.Length; i++)
        {
            newTriangles[i] = map[triangles[i]];
        }

        return newTriangles;
    }
}