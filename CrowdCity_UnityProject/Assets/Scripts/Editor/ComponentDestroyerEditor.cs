using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ComponentDestroyer))]
public class ComponentDestroyerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ComponentDestroyer myScript = (ComponentDestroyer)target;
        if (GUILayout.Button("Destroy Colliders")) myScript.DestroyColliders();
    }
}
