using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimplfyMesh))]
public class SimplfyMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SimplfyMesh mesh = (SimplfyMesh)target;
        if (GUILayout.Button("Simplify"))
        {
            mesh.SendMessage("SimplifyMesh");
        }
        if (GUILayout.Button("Reset"))
        {
            mesh.SendMessage("ResetMesh");
        }
    }
}
