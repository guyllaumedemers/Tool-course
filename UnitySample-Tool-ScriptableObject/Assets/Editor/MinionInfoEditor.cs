using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MinionInfo))]
public class MinionInfoEditor : Editor
{
    SerializedObject s_object;
    SerializedProperty p_image;
    SerializedProperty p_flag;
    SerializedProperty p_name;
    SerializedProperty p_description;
    SerializedProperty p_health;
    SerializedProperty p_mana;
    SerializedProperty p_cost;
    Texture texture;
    GUIStyle bold_style = new GUIStyle();

    readonly string SPRITE_IMAGE = "image";
    readonly string NAME = "name";
    readonly string DESCRIPTION = "description";
    readonly string HEALTH = "health";
    readonly string MANA = "defense";
    readonly string COST = "cost";
    readonly string FLAG = "flag";

    float space = 20.0f;

    private void OnEnable()
    {
        s_object = new SerializedObject(target);
        InitializeProperties();
        InitializeStyle();
        texture = AssetPreview.GetAssetPreview(p_image.objectReferenceValue);
    }

    public override void OnInspectorGUI()
    {
        s_object.Update();

        EditorGUIUtility.labelWidth = 100;

        EditorGUILayout.BeginVertical();
        p_flag.boolValue = GUILayout.Toggle(p_flag.boolValue, new GUIContent("Flag"));
        if (p_flag.boolValue)
        {
            ///// Select Texture
            EditorGUILayout.ObjectField(p_image, new GUIContent("Image"));
        }
        texture = AssetPreview.GetAssetPreview(p_image.objectReferenceValue);
        ////// Class Image
        GUILayout.Label(texture, bold_style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        ////// Class Name
        GUILayout.Space(space);
        p_name.stringValue = EditorGUILayout.TextField(p_name.stringValue, bold_style);
        ((MinionInfo)target).name = p_name.stringValue;
        GUILayout.Space(space);

        ///// Health
        p_health.intValue = EditorGUILayout.IntField(new GUIContent("Health"), p_health.intValue);
        ///// Mana
        p_mana.intValue = EditorGUILayout.IntField(new GUIContent("Mana"), p_mana.intValue);
        ///// Cost
        p_cost.intValue = EditorGUILayout.IntField(new GUIContent("Cost"), p_cost.intValue);
        ///// Minion Description
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Description"));
        p_description.stringValue = GUILayout.TextArea(p_description.stringValue);
        EditorGUILayout.EndHorizontal();

        ////// Apply Button
        if (GUILayout.Button(new GUIContent("Apply")))
            Save();
        EditorGUILayout.EndVertical();

        s_object.ApplyModifiedProperties();
    }

    private void InitializeProperties()
    {
        p_image = s_object.FindProperty(SPRITE_IMAGE);
        p_name = s_object.FindProperty(NAME);
        p_description = s_object.FindProperty(DESCRIPTION);
        p_health = s_object.FindProperty(HEALTH);
        p_mana = s_object.FindProperty(MANA);
        p_cost = s_object.FindProperty(COST);
        p_flag = s_object.FindProperty(FLAG);
    }

    private void InitializeStyle()
    {
        bold_style.fontSize = 24;
        bold_style.normal.textColor = Color.white;
        bold_style.fontStyle = FontStyle.Bold;
        bold_style.alignment = TextAnchor.MiddleCenter;
        bold_style.imagePosition = ImagePosition.ImageAbove;
    }

    private void Save()
    {
        Debug.Log("ScriptableObject saved");
    }
}
