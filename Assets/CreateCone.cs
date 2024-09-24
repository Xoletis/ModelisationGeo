using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CreateCone : MonoBehaviour
{
    public float rayon = 1.0f;
    public float hauteur = 2.0f;
    public int nbMeridiens = 20;

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int nbSommets = nbMeridiens + 2;
        Vector3[] vertices = new Vector3[nbSommets + 1];
        Vector2[] uv = new Vector2[nbSommets + 1];
        int[] triangles = new int[nbMeridiens * 3 + nbMeridiens * 3];

        vertices[0] = new Vector3(0, hauteur, 0);
        uv[0] = new Vector2(0.5f, 1f);

        vertices[nbMeridiens + 1] = new Vector3(0, 0, 0);
        uv[nbMeridiens + 1] = new Vector2(0.5f, 0f);

        for (int j = 0; j < nbMeridiens; j++)
        {
            float angle = 2 * Mathf.PI * j / nbMeridiens;
            float x = rayon * Mathf.Cos(angle);
            float z = rayon * Mathf.Sin(angle);

            vertices[j + 1] = new Vector3(x, 0, z);
            uv[j + 1] = new Vector2((float)j / nbMeridiens, 0f);
        }

        for (int j = 0; j < nbMeridiens; j++)
        {
            int a = 0;
            int b = j + 1;
            int c = (j + 1) % nbMeridiens + 1;

            triangles[j * 3] = a;
            triangles[j * 3 + 1] = c;
            triangles[j * 3 + 2] = b;
        }

        for (int j = 0; j < nbMeridiens; j++)
        {
            int b = j + 1;
            int c = (j + 1) % nbMeridiens + 1;
            int centre = nbMeridiens + 1;

            triangles[nbMeridiens * 3 + j * 3] = centre;
            triangles[nbMeridiens * 3 + j * 3 + 1] = b;
            triangles[nbMeridiens * 3 + j * 3 + 2] = c;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
