using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Building building = (Building)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Fix Colliders"))
        {
            building.FixColliders();

            EditorUtility.SetDirty(building);
            PrefabUtility.RecordPrefabInstancePropertyModifications(building);

            serializedObject.ApplyModifiedProperties();
        }
    }
}


