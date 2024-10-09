using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshLoader))]
public class MeshLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MeshLoader meshLoader = (MeshLoader)target;
        if (GUILayout.Button("Save Mesh"))
        {
            meshLoader.SendMessage("SaveMesh");
        }
    }
}
