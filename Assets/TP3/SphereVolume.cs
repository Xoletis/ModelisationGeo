using UnityEngine;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Security;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereVolume : MonoBehaviour
{
    public enum Operation
    {
        Union,
        Intersection
    }

    public List<Sphere> spheres; // Liste des sphères à afficher
    public int resolution = 10; // Nombre de subdivisions dans chaque direction

     public Operation operation = Operation.Union; // Chosen operation

    private Mesh mesh;
    private bool[,,] cubeInsideSphere; // Stocker si un cube est à l'intérieur de la sphère

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        Generate();
    }

    void Generate()
    {

        // Initialiser le mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        vertices = new List<Vector3>();
        triangles = new List<int>();

        // Calculer la taille de la grille
        int gridSize = resolution * spheres.Count; // Ajustez en fonction de votre besoin
        cubeInsideSphere = new bool[gridSize, gridSize, gridSize];

        // Construire les sphères sur la même grille
        foreach (Sphere sphere in spheres)
        {
            CreateSphereVolumeMesh(sphere.radius, sphere.center);
        }

        // Appliquer les vertices et triangles au mesh
        mesh.vertices = this.vertices.ToArray();
        mesh.triangles = this.triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void CreateSphereVolumeMesh(float _radius, Vector3 center)
    {
        Debug.Log("Radius: " + _radius);
        Debug.Log("Center: " + center);

        // Définir les limites de la boîte englobante
        float stepSize = (2 * _radius) / resolution; // Taille d'un cube (sous-volume)

        // Première passe : déterminer quels cubes sont à l'intérieur de la sphère
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    // Calculer la position du centre de chaque cube
                    Vector3 cubePosition = new Vector3(
                        center.x - _radius + (x + 0.5f) * stepSize,
                        center.y - _radius + (y + 0.5f) * stepSize,
                        center.z - _radius + (z + 0.5f) * stepSize
                    );

                    // Vérifier si ce cube est à l'intérieur de la sphère
                    cubeInsideSphere[x, y, z] = Vector3.Distance(cubePosition, center) <= _radius;
                }
            }
        }

        // Deuxième passe : générer uniquement les faces extérieures
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    // Si le cube est à l'intérieur de la sphère
                    if (cubeInsideSphere[x, y, z])
                    {
                        Vector3 cubePosition = new Vector3(
                            center.x - _radius + (x + 0.5f) * stepSize,
                            center.y - _radius + (y + 0.5f) * stepSize,
                            center.z - _radius + (z + 0.5f) * stepSize
                        );

                        // Ajouter uniquement les faces qui ne sont pas partagées avec des voisins
                        AddExposedFaces(vertices, triangles, cubePosition, stepSize, x, y, z);
                    }
                }
            }
        }
    }

    void AddExposedFaces(List<Vector3> vertices, List<int> triangles, Vector3 position, float size, int x, int y, int z)
    {
        // Positions relatives des sommets d'un cube
        Vector3[] cubeVertices = {
            new Vector3(-1, -1, -1), new Vector3( 1, -1, -1), new Vector3( 1,  1, -1), new Vector3(-1,  1, -1),
            new Vector3(-1, -1,  1), new Vector3( 1, -1,  1), new Vector3( 1,  1,  1), new Vector3(-1,  1,  1),
        };

        // Indices des triangles pour former chaque face du cube
        int[][] cubeFaces = {
            new int[] { 0, 2, 1, 0, 3, 2 }, // Face avant
            new int[] { 4, 5, 6, 4, 6, 7 }, // Face arrière
            new int[] { 0, 7, 3, 0, 4, 7 }, // Face gauche
            new int[] { 1, 2, 6, 1, 6, 5 }, // Face droite
            new int[] { 3, 7, 6, 3, 6, 2 }, // Face haut
            new int[] { 0, 1, 5, 0, 5, 4 }  // Face bas
        };

        // Vérifier les voisins dans chaque direction (x, y, z)
        if (x == 0 || !cubeInsideSphere[x - 1, y, z]) AddFace(vertices, triangles, position, size, cubeVertices, cubeFaces[2]); // Face gauche
        if (x == resolution - 1 || !cubeInsideSphere[x + 1, y, z]) AddFace(vertices, triangles, position, size, cubeVertices, cubeFaces[3]); // Face droite
        if (y == 0 || !cubeInsideSphere[x, y - 1, z]) AddFace(vertices, triangles, position, size, cubeVertices, cubeFaces[5]); // Face bas
        if (y == resolution - 1 || !cubeInsideSphere[x, y + 1, z]) AddFace(vertices, triangles, position, size, cubeVertices, cubeFaces[4]); // Face haut
        if (z == 0 || !cubeInsideSphere[x, y, z - 1]) AddFace(vertices, triangles, position, size, cubeVertices, cubeFaces[0]); // Face avant
        if (z == resolution - 1 || !cubeInsideSphere[x, y, z + 1]) AddFace(vertices, triangles, position, size, cubeVertices, cubeFaces[1]); // Face arrière
    }

    void AddFace(List<Vector3> vertices, List<int> triangles, Vector3 position, float size, Vector3[] cubeVertices, int[] faceIndices)
    {
        int vertexIndex = vertices.Count;

        // Ajouter les vertices pour cette face
        for (int i = 0; i < 8; i++)
        {
            vertices.Add(position + (cubeVertices[i] * size * 0.5f)); // Ajouter chaque sommet avec l'échelle
        }

        // Ajouter les triangles pour cette face
        for (int i = 0; i < faceIndices.Length; i++)
        {
            triangles.Add(vertexIndex + faceIndices[i]);
        }
    }
}