using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CarGroup))]
public class CarGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CarGroup carGroup = (CarGroup)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Replace With Prefabs"))
        {
            Debug.Log($"Replace With Prefabs CALLED for {carGroup.name}");

            carGroup.ReplaceWithPrefabs();
        }

        if (GUILayout.Button("Randomize Materials"))
        {
            Debug.Log($"Randomize Materials CALLED for {carGroup.name}");

            carGroup.RandomizeMaterials();
        }
    }
}
