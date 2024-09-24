using UnityEngine;

public class Sphere : MonoBehaviour
{
    public float rayon = 1.0f;
    public int nbParalleles = 10;
    public int nbMeridiens = 20;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int nbSommets = (nbParalleles + 1) * (nbMeridiens + 1);
        Vector3[] vertices = new Vector3[nbSommets];
        Vector2[] uv = new Vector2[nbSommets];
        int[] triangles = new int[nbParalleles * nbMeridiens * 6];

        int vertIndex = 0;
        for (int i = 0; i <= nbParalleles; i++)
        {
            float latitude = Mathf.PI * (-0.5f + (float)i / nbParalleles);
            for (int j = 0; j <= nbMeridiens; j++)
            {
                float longitude = 2 * Mathf.PI * (float)j / nbMeridiens;
                float x = rayon * Mathf.Cos(latitude) * Mathf.Cos(longitude);
                float y = rayon * Mathf.Sin(latitude);
                float z = rayon * Mathf.Cos(latitude) * Mathf.Sin(longitude);

                vertices[vertIndex] = new Vector3(x, y, z);
                uv[vertIndex] = new Vector2((float)j / nbMeridiens, (float)i / nbParalleles);
                vertIndex++;
            }
        }

        int triIndex = 0;
        for (int i = 0; i < nbParalleles; i++)
        {
            for (int j = 0; j < nbMeridiens; j++)
            {
                int a = i * (nbMeridiens + 1) + j;
                int b = a + nbMeridiens + 1;
                int c = a + 1;
                int d = b + 1;

                triangles[triIndex++] = a;
                triangles[triIndex++] = b;
                triangles[triIndex++] = c;

                triangles[triIndex++] = b;
                triangles[triIndex++] = d;
                triangles[triIndex++] = c;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
