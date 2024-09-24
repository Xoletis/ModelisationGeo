using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public int nombre_Lignes = 10;
    public int nb_Colonnes = 10;
    public float taille_Case = 1.0f;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[(nombre_Lignes + 1) * (nb_Colonnes + 1)];
        Vector2[] uv = new Vector2[(nombre_Lignes + 1) * (nb_Colonnes + 1)];
        int[] triangles = new int[nombre_Lignes * nb_Colonnes * 6];

        // Génération des sommets et des UV
        for (int i = 0; i <= nombre_Lignes; i++)
        {
            for (int j = 0; j <= nb_Colonnes; j++)
            {
                int index = i * (nb_Colonnes + 1) + j;
                vertices[index] = new Vector3(j * taille_Case, 0, i * taille_Case);
                uv[index] = new Vector2((float)j / nb_Colonnes, (float)i / nombre_Lignes);
            }
        }

        int triangleIndex = 0;
        for (int i = 0; i < nombre_Lignes; i++)
        {
            for (int j = 0; j < nb_Colonnes; j++)
            {
                int indexA = i * (nb_Colonnes + 1) + j;
                int indexB = indexA + nb_Colonnes + 1;
                int indexC = indexA + 1;
                int indexD = indexB + 1;

                AddTriangle(triangles, ref triangleIndex, indexA, indexB, indexC);
                AddTriangle(triangles, ref triangleIndex, indexC, indexB, indexD);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void AddTriangle(int[] triangles, ref int triangleIndex, int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }
}
