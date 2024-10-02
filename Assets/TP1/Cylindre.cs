using UnityEngine;

public class Cylindre : MonoBehaviour
{
    public float rayon = 1.0f;
    public float hauteur = 2.0f;
    public int meridiensNb = 20;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[2 * (meridiensNb + 1) + 2];
        int[] triangles = new int[meridiensNb * 12];

        int vertIndex = 0;
        float angleStep = 2 * Mathf.PI / meridiensNb;
        for (int i = 0; i <= meridiensNb; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle) * rayon;
            float z = Mathf.Sin(angle) * rayon;

            vertices[vertIndex] = new Vector3(x, 0, z);
            vertIndex++;

            vertices[vertIndex] = new Vector3(x, hauteur, z);
            vertIndex++;
        }

        vertices[vertIndex] = new Vector3(0, 0, 0);
        int baseInferieureCentreIndex = vertIndex;
        vertIndex++;

        vertices[vertIndex] = new Vector3(0, hauteur, 0);
        int baseSuperieureCentreIndex = vertIndex;
        vertIndex++;

        int triIndex = 0;
        for (int i = 0; i < meridiensNb; i++)
        {
            int baseA = i * 2;
            int baseB = (i + 1) * 2;

            AddTriangle(triangles, ref triIndex, baseA + 1, baseB, baseA);
            AddTriangle(triangles, ref triIndex, baseA + 1, baseB + 1, baseB);
        }

        for (int i = 0; i < meridiensNb; i++)
        {
            int baseA = i * 2;
            int baseB = (i + 1) * 2;
            AddTriangle(triangles, ref triIndex, baseInferieureCentreIndex, baseA, baseB);
        }

        for (int i = 0; i < meridiensNb; i++)
        {
            int baseA = i * 2 + 1;
            int baseB = (i + 1) * 2 + 1;
            AddTriangle(triangles, ref triIndex, baseSuperieureCentreIndex, baseB, baseA);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void AddTriangle(int[] triangles, ref int triIndex, int a, int b, int c)
    {
        triangles[triIndex] = a;
        triangles[triIndex + 1] = b;
        triangles[triIndex + 2] = c;
        triIndex += 3;
    }
}
