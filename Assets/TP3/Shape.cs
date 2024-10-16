using UnityEngine;


[System.Serializable]
public abstract class Shape
{
    public abstract bool IsInside(Vector3 point); // V�rifie si un point est � l'int�rieur de la forme
    public abstract Vector3 GetBoundingBoxMin(); // Obtenir le coin inf�rieur de la bounding box
    public abstract Vector3 GetBoundingBoxMax(); // Obtenir le coin sup�rieur de la bounding box
}
