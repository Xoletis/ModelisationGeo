using UnityEngine;
using System.Collections.Generic;

public class SphereVolume : MonoBehaviour
{
    public enum Operation
    {
        Union,
        Intersection
    }

    public static SphereVolume Instance;

    public int visiblePotentiel = 200;

    public List<Sphere> spheres;
    public int resolution = 10;

    public Vector3 size = new Vector3(10, 10, 10);

    public Operation operation = Operation.Union;

    public GameObject cubePrefab;

    private bool[,,] cubeInsideSphere;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Il y a plusieurs instances de SphereVolume");
        }

        Instance = this;
    }

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("Cube Prefab is not assigned!");
            return;
        }

        for (int i = 0; i < transform.childCount; i++) 
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        switch (operation)
        {
            case Operation.Union:
                foreach (Sphere sphere in spheres)
                {
                    CreateSphereVolume(sphere.radius, sphere.center);
                } break;
            case Operation.Intersection: CreateIntersectionVolume(); break;

        }
    }

    void CreateSphereVolume(float _radius, Vector3 center)
    {
        float stepSize = (2 * _radius) / resolution;
        float radiusSquared = _radius * _radius;

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    Vector3 cubePosition = new Vector3(
                        center.x - _radius + (x + 0.5f) * stepSize,
                        center.y - _radius + (y + 0.5f) * stepSize,
                        center.z - _radius + (z + 0.5f) * stepSize
                    );

                    float deltaX = cubePosition.x - center.x;
                    float deltaY = cubePosition.y - center.y;
                    float deltaZ = cubePosition.z - center.z;

                    if ((deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ) < radiusSquared)
                    {
                        InstantiateCube(cubePosition, stepSize);
                    }
                }
            }
        }
    }

    void CreateIntersectionVolume()
    {
        if (spheres.Count < 2) return;

        Vector3 min = spheres[0].center - Vector3.one * spheres[0].radius;
        Vector3 max = spheres[0].center + Vector3.one * spheres[0].radius;

        foreach (Sphere sphere in spheres)
        {
            Vector3 sphereMin = sphere.center - Vector3.one * sphere.radius;
            Vector3 sphereMax = sphere.center + Vector3.one * sphere.radius;

            min = Vector3.Min(min, sphereMin);
            max = Vector3.Max(max, sphereMax);
        }

        float stepSize = (max - min).magnitude / resolution;

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    Vector3 cubePosition = new Vector3(
                        min.x + (x + 0.5f) * stepSize,
                        min.y + (y + 0.5f) * stepSize,
                        min.z + (z + 0.5f) * stepSize
                    );

                    if (IsInsideAtLeastTwoSpheres(cubePosition))
                    {
                        InstantiateCube(cubePosition, stepSize);
                    }
                }
            }
        }
    }

    bool IsInsideAtLeastTwoSpheres(Vector3 point)
    {
        int count = 0;

        foreach (Sphere sphere in spheres)
        {
            float deltaX = point.x - sphere.center.x;
            float deltaY = point.y - sphere.center.y;
            float deltaZ = point.z - sphere.center.z;
            float distanceSquared = deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;

            if (distanceSquared <= sphere.radius * sphere.radius)
            {
                count++;
                if (count >= 2)
                {
                    return true;
                }
            }
        }
        return false;
    }


    void InstantiateCube(Vector3 position, float size)
    {
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity, transform);
        cube.transform.localScale = Vector3.one * size;
    }
}
