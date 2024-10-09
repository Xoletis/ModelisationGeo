using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System;

public class MeshLoader : MonoBehaviour
{
    // Path vers le fichier .off (vous pouvez aussi passer ce path dynamiquement)
    public string filePath = "./buddha.off";
    public bool showVerticeAndFaceCoordinante = false;
    public bool debug = false;

    // Le mesh Unity que nous allons créer
    private Mesh mesh;

    void Start()
    {
        // Appel de la méthode pour charger le fichier .off
        LoadMeshFromFile(filePath);
    }

    void LoadMeshFromFile(string path)
    {
        // Lecture du fichier
        string[] lines = File.ReadAllLines(path);

        // Vérification de l'extension
        if (lines[0] != "OFF")
        {
            Debug.LogError("Le fichier n'est pas un fichier OFF valide.");
            return;
        }

        // Extraction du nombre de sommets, facettes et arêtes (on ignore les arêtes)
        string[] counts = lines[1].Split(' ');
        int numVertices = int.Parse(counts[0]);
        int numFaces = int.Parse(counts[1]);

        // Initialisation des listes de sommets et facettes
        Vector3[] vertices = new Vector3[numVertices];
        List<int> triangles = new List<int>();

        // Parsing des sommets (ligne 2 à 2 + numVertices)
        for (int i = 0; i < numVertices; i++)
        {
            string[] vertexData = lines[i + 2].Split(' ');
            // CultureInfo.InvariantCulture assure que le point est utilisé comme séparateur décimal
            float x = float.Parse(vertexData[0], CultureInfo.InvariantCulture);
            float y = float.Parse(vertexData[1], CultureInfo.InvariantCulture);
            float z = float.Parse(vertexData[2], CultureInfo.InvariantCulture);
            vertices[i] = new Vector3(x, y, z);
        }

        // Parsing des facettes (lignes après les sommets)
        for (int i = 0; i < numFaces; i++)
        {
            string[] faceData = lines[i + 2 + numVertices].Split(' ');

            // On suppose que toutes les facettes sont triangulaires (valeur 3)
            if (faceData[0] != "3")
            {
                Debug.LogError("Le fichier contient des facettes non triangulaires.");
                return;
            }

            int v0 = int.Parse(faceData[1]);
            int v1 = int.Parse(faceData[2]);
            int v2 = int.Parse(faceData[3]);

            // Ajout des indices des sommets pour former un triangle
            triangles.Add(v0);
            triangles.Add(v1);
            triangles.Add(v2);
        }

        CenterMesh(vertices);
        NormalizeSize(vertices);

        // Création d'un mesh Unity
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();

        if (showVerticeAndFaceCoordinante) TraceMaillage(mesh.vertices, mesh.triangles);

        // Optionnel : recalculer les normales pour un meilleur rendu
        mesh.RecalculateNormals();

        // Assigner le mesh au MeshFilter et au MeshRenderer
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh = mesh;
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            gameObject.AddComponent<MeshRenderer>();
        }
    }


    public void TraceMaillage(Vector3[] vertices, int[] triangles)
    {
        // Afficher les sommets
        Debug.Log("Sommets :");
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            Debug.Log("Sommet " + i + ": (" + vertex.x + ", " + vertex.y + ", " + vertex.z + ")");
        }

        // Afficher les triangles
        Debug.Log("Triangles :");
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int v0 = triangles[i];
            int v1 = triangles[i + 1];
            int v2 = triangles[i + 2];
            Debug.Log("Triangle " + (i / 3) + ": " + v0 + ", " + v1 + ", " + v2);
        }
    }

   void CenterMesh(Vector3[] vertices)
   {
        // Calcul du centre de gravité
        Vector3 centerOfMass = Vector3.zero;  // Initialisation du centre de gravité à (0, 0, 0)

        foreach (Vector3 vertex in vertices)
        {
            centerOfMass += vertex;  // Somme des coordonnées des sommets
        }

        centerOfMass /= vertices.Length;  // Moyenne des coordonnées, soit le centre de gravité

        // Recentrer les sommets autour de (0, 0, 0)
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= centerOfMass;  // Déplace chaque sommet en soustrayant le centre de gravité
        }
   }

    void NormalizeSize(Vector3[] vertices)
    {
        // Trouver la plus grande coordonnée en valeur absolue
        float maxCoordinate = 0f;

        foreach (Vector3 vertex in vertices)
        {
            maxCoordinate = Mathf.Max(maxCoordinate, Mathf.Abs(vertex.x), Mathf.Abs(vertex.y), Mathf.Abs(vertex.z));
        }

        // Normaliser toutes les coordonnées pour qu'elles soient comprises entre -1 et 1
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] /= maxCoordinate;
        }
    }

    public static void SaveMeshToFile(Mesh mesh, string filePath)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.Write(MeshToString(mesh));
        }
        Debug.Log("Mesh saved to " + filePath);
    }

    private static string MeshToString(Mesh mesh)
    {
        StringWriter meshString = new StringWriter();
        meshString.WriteLine("# Unity mesh generated");

        // Export vertices
        foreach (Vector3 v in mesh.vertices)
        {
            meshString.WriteLine(string.Format(CultureInfo.InvariantCulture, "v {0} {1} {2}", v.x, v.y, v.z));
        }

        // Export normals
        foreach (Vector3 n in mesh.normals)
        {
            meshString.WriteLine(string.Format(CultureInfo.InvariantCulture, "vn {0} {1} {2}", n.x, n.y, n.z));
        }

        // Export UVs
        foreach (Vector2 uv in mesh.uv)
        {
            meshString.WriteLine(string.Format(CultureInfo.InvariantCulture, "vt {0} {1}", uv.x, uv.y));
        }

        // Export triangles
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            // OBJ format starts vertex indices at 1
            meshString.WriteLine(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}",
                mesh.triangles[i] + 1, mesh.triangles[i + 1] + 1, mesh.triangles[i + 2] + 1));
        }

        return meshString.ToString();
    }

    // Example usage
    [ContextMenu("Save Mesh To Obj")]
    private void SaveMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;  // Get the mesh from the object
        string path = Application.dataPath + "/SavedMesh.obj"; // Save location
        SaveMeshToFile(mesh, path);
        Debug.Log("saved");
    }

    void Update()
    {
        // Vérifier si la touche S est pressée
        if (Input.GetKeyDown(KeyCode.S))
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            string path = Application.dataPath + "/SavedMesh.obj";
            SaveMeshToFile(mesh, path);
        }
    }
}
