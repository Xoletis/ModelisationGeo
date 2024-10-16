using UnityEngine;


[System.Serializable]
public class Quadric : Shape
{
    public float A, B, C, D, E, F, G, H, I, J; // Coefficients de la quadrique
    public Vector3 center; // Centre de la quadrique (pour la translation si nécessaire)

    public Quadric(float a, float b, float c, float d, float e, float f, float g, float h, float i, float j, Vector3 center)
    {
        A = a; B = b; C = c; D = d; E = e; F = f; G = g; H = h; I = i; J = j;
        this.center = center;
    }

    // Implémentation de la méthode IsInside pour une quadrique
    public override bool IsInside(Vector3 point)
    {
        // Appliquer la translation pour centrer la quadrique
        float x = point.x - center.x;
        float y = point.y - center.y;
        float z = point.z - center.z;

        // Calculer la valeur de l'équation de la quadrique
        float result = A * x * x + B * y * y + C * z * z + D * x * y + E * x * z + F * y * z + G * x + H * y + I * z + J;

        return result <= 0; // À l'intérieur si <= 0
    }

    public override Vector3 GetBoundingBoxMin()
    {
        // Approximation de la bounding box pour une quadrique
        float width = Mathf.Abs(G) + Mathf.Sqrt(A * 4); // Approximation simple
        float height = Mathf.Abs(H) + Mathf.Sqrt(B * 4);
        float depth = Mathf.Abs(I) + Mathf.Sqrt(C * 4);

        return center - new Vector3(width, height, depth) * 0.5f; // Coin inférieur
    }

    public override Vector3 GetBoundingBoxMax()
    {
        // Approximation de la bounding box pour une quadrique
        float width = Mathf.Abs(G) + Mathf.Sqrt(A * 4); // Approximation simple
        float height = Mathf.Abs(H) + Mathf.Sqrt(B * 4);
        float depth = Mathf.Abs(I) + Mathf.Sqrt(C * 4);

        return center + new Vector3(width, height, depth) * 0.5f; // Coin supérieur
    }
}
