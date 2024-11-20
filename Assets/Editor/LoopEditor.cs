using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LoopSubdivision))]
public class LoopSubdivisionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LoopSubdivision sphere = (LoopSubdivision)target;
        if (GUILayout.Button("Generate"))
        {
            sphere.SendMessage("Generate");
        }

        if (GUILayout.Button("Reset"))
        {
            sphere.SendMessage("ResetMesh");
        }
    }
}
