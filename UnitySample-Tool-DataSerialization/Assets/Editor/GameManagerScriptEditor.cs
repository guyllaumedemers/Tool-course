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
    private SerializedProperty serializationType;

    [Header("Properties strings")]
    private readonly string nb_shapes_path = "nb_shapes";
    private readonly string meshType_path = "meshType";
    private readonly string serializationType_path = "serializationType";

    private string[] meshType_enums;
    private string[] serializationType_enums;
    private int meshType_index = 3;
    private int serializationType_index = 0;

    [Header("Constant values")]
    private const int MIN_VALUE = 0;
    private const int MAX_VALUE = 10;

    private void OnEnable()
    {
        managerObj = new SerializedObject(target);
        InitializeProperties();
        meshType_enums = meshType.enumNames;
        serializationType_enums = serializationType.enumNames;
    }

    private void InitializeProperties()
    {
        nb_shapes = managerObj.FindProperty(nb_shapes_path);
        meshType = managerObj.FindProperty(meshType_path);
        serializationType = managerObj.FindProperty(serializationType_path);
    }

    public override void OnInspectorGUI()
    {
        managerObj.Update();
        GUILayout.BeginVertical();

        EditorGUILayout.PropertyField(nb_shapes, new GUIContent("Number Shapes"));
        if (nb_shapes.intValue < MIN_VALUE)
            nb_shapes.intValue = MIN_VALUE;
        else if (nb_shapes.intValue > MAX_VALUE)
            nb_shapes.intValue = MAX_VALUE;

        meshType_index = EditorGUILayout.Popup(new GUIContent("Shape Selection"), meshType_index, meshType_enums);
        GameManagerScript.Instance.GetMeshType = (EnumMeshType)meshType_index;

        if (GUILayout.Button(new GUIContent("Create"), GUILayout.ExpandWidth(true)))
            GameManagerScript.Instance.InstanciateShapes(nb_shapes.intValue);

        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Save"), GUILayout.ExpandWidth(true)))
            GameManagerScript.Instance.OnSerialization();
        if (GUILayout.Button(new GUIContent("Load"), GUILayout.ExpandWidth(true)))
            GameManagerScript.Instance.OnDeserilization();
        GUILayout.EndHorizontal();

        serializationType_index = EditorGUILayout.Popup(new GUIContent("Serialization Type"), serializationType_index, serializationType_enums);
        GameManagerScript.Instance.GetSerializationType = (EnumSerializationType)serializationType_index;

        GUILayout.EndVertical();
        managerObj.ApplyModifiedProperties();
    }
}
