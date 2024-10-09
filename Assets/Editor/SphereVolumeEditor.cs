using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SphereVolume))]
public class SphereVolumeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SphereVolume sphere = (SphereVolume)target;
        if (GUILayout.Button("Generate"))
        {
            sphere.SendMessage("Generate");
        }
    }
}
