using System;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public float rayon = 1.0f;
    public int parallelesNb = 10;
    public int meridiensNb = 20;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int nbSommets = (parallelesNb + 1) * (meridiensNb + 1);
        Vector3[] vertices = new Vector3[nbSommets];
        int[] triangles = new int[parallelesNb * meridiensNb * 6];

        int vertIndex = 0;
        for (int i = 0; i <= parallelesNb; i++)
        {
            float latitude = Mathf.PI * (-0.5f + (float)i / parallelesNb);
            for (int j = 0; j <= meridiensNb; j++)
            {
                float longitude = 2 * Mathf.PI * (float)j / meridiensNb;
                float x = rayon * Mathf.Cos(latitude) * Mathf.Cos(longitude);
                float y = rayon * Mathf.Sin(latitude);
                float z = rayon * Mathf.Cos(latitude) * Mathf.Sin(longitude);

                vertices[vertIndex] = new Vector3(x, y, z);
                vertIndex++;
            }
        }

        int triangleIndex = 0;
        for (int i = 0; i < parallelesNb; i++)
        {
            for (int j = 0; j < meridiensNb; j++)
            {
                int indexA = i * (meridiensNb + 1) + j;
                int indexB = indexA + meridiensNb + 1;
                int indexC = indexA + 1;
                int indexD = indexB + 1;
                AddTriangle(triangles, ref triangleIndex, indexA, indexB, indexC);
                AddTriangle(triangles, ref triangleIndex, indexC, indexB, indexD);
            }
        }
        mesh.vertices = vertices;
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
