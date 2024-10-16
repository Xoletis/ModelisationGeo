using UnityEngine;


[System.Serializable]
public abstract class Shape
{
    public abstract bool IsInside(Vector3 point); // Vérifie si un point est à l'intérieur de la forme
    public abstract Vector3 GetBoundingBoxMin(); // Obtenir le coin inférieur de la bounding box
    public abstract Vector3 GetBoundingBoxMax(); // Obtenir le coin supérieur de la bounding box
}
