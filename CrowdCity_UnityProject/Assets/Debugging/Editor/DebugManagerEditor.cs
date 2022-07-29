#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DebugManager))]
public class DebugManagerEditor : Editor
{
    private ReorderableList reorderableList;

    private DebugManager debugManager
    {
        get
        {
            return target as DebugManager;
        }
    }

    private void OnEnable()
    {
        reorderableList = new ReorderableList(debugManager.settingsItemScripts, typeof(DebugManager), true, true, false, false);
        RefreshSettingItems();
        reorderableList.headerHeight = EditorGUIUtility.singleLineHeight * 2;
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight;
        // This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
        // Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
        // which is a UnityEngine.Object
        // reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);
        reorderableList.onChangedCallback += (ReorderableList rl) =>
        {
            serializedObject.Update();
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        };
        
        // Add listeners to draw events
        reorderableList.drawHeaderCallback += DrawHeader;
        reorderableList.drawElementCallback += DrawElement;

        reorderableList.onAddCallback += AddItem;
        reorderableList.onRemoveCallback += RemoveItem;
    }

    private void OnDisable()
    {
        // Make sure we don't get memory leaks etc.
        reorderableList.drawHeaderCallback -= DrawHeader;
        reorderableList.drawElementCallback -= DrawElement;

        reorderableList.onAddCallback -= AddItem;
        reorderableList.onRemoveCallback -= RemoveItem;
    }

    /// <summary>
    /// Draws the header of the list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeader(Rect rect)
    {
        GUI.Label(new Rect(rect.x, rect.y, rect.width, rect.height / 2), "Settings Item order in debug menu");
        

    }

    /// <summary>
    /// Draws one element of the list (ListItemExample)
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="active"></param>
    /// <param name="focused"></param>
    private void DrawElement(Rect rect, int index, bool active, bool focused)
    {
        serializedObject.Update();

        SettingsItemScript item = debugManager.settingsItemScripts[index];
        SerializedProperty elements = serializedObject.FindProperty(nameof(debugManager.settingsItemScripts));
        SerializedProperty element = elements.GetArrayElementAtIndex(index);

        EditorGUI.BeginChangeCheck();

        if(item != null)
        {
            //EditorGUI.LabelField(new Rect(rect.x, rect.y, 18, rect.height), (index + 1).ToString());
            EditorGUI.LabelField(new Rect(rect.x + 18, rect.y, (rect.width - 18) /2, rect.height), item.title);
            //EditorGUI.ObjectField(new Rect(rect.x + 18 + (rect.width - 18) / 2, rect.y, (rect.width - 18) / 2, rect.height), element);

        }
        else
        {
            Debug.LogWarning("Please refresh your debug manager list (click on refresh Button from DebugManager)");
        }

       
        serializedObject.ApplyModifiedProperties();

        // If you are using a custom PropertyDrawer, this is probably better
        // EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
        // Although it is probably smart to cach the list as a private variable ;)
    }

    private void AddItem(ReorderableList list)
    {
        //debugManager.sett.Add(new Level());
        Debug.Log("Can't add an item");
        EditorUtility.SetDirty(target);
    }

    private void RemoveItem(ReorderableList list)
    {
        //debugManager.levels.RemoveAt(list.index);
        Debug.Log("Can't remove an item");
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Actually draw the list in the inspector
        //debugManager.CycleStartLevel = EditorGUILayout.(debugManager.CycleStartLevel, "CycleStartLevel");
        if (GUILayout.Button("Refresh setting items"))
        {
            Debug.Log("Refreshed the list of settings items");
            RefreshSettingItems();
        }
        

        reorderableList.DoLayoutList();
        
    }
    private void RefreshSettingItems()
    {
        debugManager.FindAllSettingsItems();
        reorderableList.list = debugManager.settingsItemScripts;
        serializedObject.Update();
        EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif