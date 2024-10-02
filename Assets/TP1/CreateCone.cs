using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CreateCone : MonoBehaviour
{
    public float rayon = 1.0f;
    public float hauteur = 2.0f;
    public int meridiensNb = 20;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int nbSommets = meridiensNb + 2;
        Vector3[] vertices = new Vector3[nbSommets + 1];
        int[] triangles = new int[meridiensNb * 3 + meridiensNb * 3];

        vertices[0] = new Vector3(0, hauteur, 0);

        vertices[meridiensNb + 1] = new Vector3(0, 0, 0);

        for (int j = 0; j < meridiensNb; j++)
        {
            float angle = 2 * Mathf.PI * j / meridiensNb;
            float x = rayon * Mathf.Cos(angle);
            float z = rayon * Mathf.Sin(angle);

            vertices[j + 1] = new Vector3(x, 0, z);
        }

        for (int j = 0; j < meridiensNb; j++)
        {
            int a = 0;
            int b = j + 1;
            int c = (j + 1) % meridiensNb + 1;

            triangles[j * 3] = a;
            triangles[j * 3 + 1] = c;
            triangles[j * 3 + 2] = b;
        }

        for (int j = 0; j < meridiensNb; j++)
        {
            int b = j + 1;
            int c = (j + 1) % meridiensNb + 1;
            int centre = meridiensNb + 1;
            triangles[meridiensNb * 3 + j * 3] = centre;
            triangles[meridiensNb * 3 + j * 3 + 1] = b;
            triangles[meridiensNb * 3 + j * 3 + 2] = c;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
