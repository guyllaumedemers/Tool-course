using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManagerScript))]
public class GameManagerScriptEditor : Editor
{
    [Header("Serialized Informations")]
    private SerializedObject managerObj;
    private SerializedProperty nb_shapes;
    private SerializedProperty meshType;
    private SerializedProperty prefabObject;

    [Header("Properties strings")]
    private readonly string nb_shapes_path = "nb_shapes";
    private readonly string meshType_path = "meshType";
    private readonly string prefab_path = "prefab";

    private string[] enums;
    private int index = 0;

    [Header("Constant values")]
    private const int MIN_VALUE = 0;
    private const int MAX_VALUE = 10;

    private void OnEnable()
    {
        managerObj = new SerializedObject(target);
        InitializeProperties();
        enums = meshType.enumNames;
    }

    private void InitializeProperties()
    {
        nb_shapes = managerObj.FindProperty(nb_shapes_path);
        meshType = managerObj.FindProperty(meshType_path);
        prefabObject = managerObj.FindProperty(prefab_path);
    }

    public override void OnInspectorGUI()
    {
        managerObj.Update();

        GUILayout.BeginVertical();
        EditorGUILayout.ObjectField(prefabObject);
        EditorGUILayout.PropertyField(nb_shapes, new GUIContent("Number Shapes"));
        if (nb_shapes.intValue < MIN_VALUE)
            nb_shapes.intValue = MIN_VALUE;
        else if (nb_shapes.intValue > MAX_VALUE)
            nb_shapes.intValue = MAX_VALUE;

        index = EditorGUILayout.Popup(new GUIContent("Shape Selection"), index, enums);
        GameManagerScript.Instance.GetMeshType = (EnumMeshType)index;

        if (GUILayout.Button(new GUIContent("Create"), GUILayout.ExpandWidth(true)))
            GameManagerScript.Instance.InstanciateShapes((GameObject)prefabObject.objectReferenceValue, nb_shapes.intValue);

        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Save"), GUILayout.ExpandWidth(true)))
            GameManagerScript.Instance.OnSerialization();
        if (GUILayout.Button(new GUIContent("Load"), GUILayout.ExpandWidth(true)))
            GameManagerScript.Instance.OnDeserilization();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        managerObj.ApplyModifiedProperties();
    }
}
