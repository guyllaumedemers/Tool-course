using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClassInfo))]
public class ClassInfoEditor : Editor
{
    SerializedObject s_object;
    SerializedProperty p_image;
    SerializedProperty p_name;
    SerializedProperty p_basic_card;
    SerializedProperty p_golden_card;
    Texture texture;
    GUIStyle bold_style = new GUIStyle();

    readonly string SPRITE_IMAGE = "image";
    readonly string NAME = "name";
    readonly string BASIC_CARDS = "basic_cards";
    readonly string GOLDEN_CARDS = "golden_class_specific_cards";

    float space = 20.0f;
    float index_offset = 1;
    Vector2 scrollview = new Vector2();

    private void OnEnable()
    {
        s_object = new SerializedObject(target);
        p_image = s_object.FindProperty(SPRITE_IMAGE);
        p_name = s_object.FindProperty(NAME);
        p_basic_card = s_object.FindProperty(BASIC_CARDS);
        p_golden_card = s_object.FindProperty(GOLDEN_CARDS);
        InitializeStyle();
        texture = AssetPreview.GetAssetPreview(p_image.objectReferenceValue);
    }

    public override void OnInspectorGUI()
    {
        s_object.Update();

        EditorGUIUtility.labelWidth = 100;

        EditorGUILayout.BeginVertical();
        scrollview = EditorGUILayout.BeginScrollView(scrollview); ///// why doesnt it update?
        ////// Class Image
        GUILayout.Label(texture, bold_style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        ////// Class Name
        GUILayout.Space(space);
        p_name.stringValue = EditorGUILayout.TextField(p_name.stringValue, bold_style);
        ((ClassInfo)target).name = p_name.stringValue;
        GUILayout.Space(space);
        ////// Basic Cards
        for (int i = 0; i < p_basic_card.arraySize; i++)
        {
            string temp = p_basic_card.GetArrayElementAtIndex(i).stringValue;
            temp = EditorGUILayout.TextField(new GUIContent($"Basic Card : {i + index_offset}"), temp);
            p_basic_card.GetArrayElementAtIndex(i).stringValue = temp;
        }
        GUILayout.Space(space);
        ////// Golden Cards
        for (int i = 0; i < p_golden_card.arraySize; i++)
        {
            string temp = p_golden_card.GetArrayElementAtIndex(i).stringValue;
            temp = EditorGUILayout.TextField(new GUIContent($"Golden Card : {i + index_offset}"), temp, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            p_golden_card.GetArrayElementAtIndex(i).stringValue = temp;
        }
        GUILayout.Space(space);
        ////// Apply Button
        if (GUILayout.Button(new GUIContent("Apply")))
            Save();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        s_object.ApplyModifiedProperties();
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
