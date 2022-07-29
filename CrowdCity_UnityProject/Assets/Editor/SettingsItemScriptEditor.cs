using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(SettingsItemScript))]
public class SettingsItemScriptEditor : Editor
{
    SerializedProperty dropDownList;
    SerializedProperty sliderEvent;
    SerializedProperty dropdownEvent;
    SerializedProperty boolEvent;
    SerializedProperty restartFunction;

    private void OnEnable()
    {
        sliderEvent = serializedObject.FindProperty("floatValueChangeCallback");
        dropdownEvent = serializedObject.FindProperty("dropdownValueChangeCallback");
        boolEvent = serializedObject.FindProperty("toggleValueChangeCallback");
        restartFunction = serializedObject.FindProperty("restartFunction");

        dropDownList = serializedObject.FindProperty("settingDropdowns");

    }
    public override void OnInspectorGUI()
    {
        SettingsItemScript settingsItemScript = (SettingsItemScript)target;
        EditorGUILayout.BeginVertical();

        settingsItemScript.title = EditorGUILayout.TextField("Title", settingsItemScript.title);
        settingsItemScript.playerPrefsName = EditorGUILayout.TextField("PlayerPrefsName", settingsItemScript.playerPrefsName);
        settingsItemScript.debugSettingsItemType = (DebugSettingsItemTypes)EditorGUILayout.EnumPopup("Debug setting type", settingsItemScript.debugSettingsItemType);
        settingsItemScript.RestartOnValueChangeFromUI = (bool)EditorGUILayout.Toggle("RestartOnValueChangeFromUI", settingsItemScript.RestartOnValueChangeFromUI);

        serializedObject.Update();


        switch (settingsItemScript.debugSettingsItemType)
        {
            case DebugSettingsItemTypes.Dropdown:
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Dropdown settings", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                settingsItemScript.currentValueInt = EditorGUILayout.IntField("Dropdown default value", settingsItemScript.currentValueInt);
                EditorGUILayout.PropertyField(dropDownList);
                EditorGUILayout.PropertyField(dropdownEvent, true);

                break;

            case DebugSettingsItemTypes.FloatSlider:
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Slider settings", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                settingsItemScript.currentValueFloatSlider = EditorGUILayout.FloatField("Slider default value", settingsItemScript.currentValueFloatSlider);
                settingsItemScript.floatSliderMinValue = EditorGUILayout.FloatField("Slider min value", settingsItemScript.floatSliderMinValue);
                settingsItemScript.floatSliderMaxValue = EditorGUILayout.FloatField("Slider max value", settingsItemScript.floatSliderMaxValue);

                EditorGUILayout.PropertyField(sliderEvent, true);

                break;
            case DebugSettingsItemTypes.Toggle:
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Toggle settings", EditorStyles.boldLabel);
                EditorGUILayout.Space();

                settingsItemScript.currentValueBool = EditorGUILayout.Toggle("Toggle default value", settingsItemScript.currentValueBool);

                EditorGUILayout.PropertyField(boolEvent, true);
                break;
        }

        if (settingsItemScript.RestartOnValueChangeFromUI == true)
        {
            EditorGUILayout.PropertyField(restartFunction, true);
        }
        serializedObject.ApplyModifiedProperties();


        EditorGUILayout.EndVertical();

    }
}
