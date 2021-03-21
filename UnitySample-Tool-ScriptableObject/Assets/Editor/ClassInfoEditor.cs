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
    SerializedProperty p_flag;
    SerializedProperty p_basic_card;
    SerializedProperty p_golden_card;
    Texture texture;
    GUIStyle bold_style = new GUIStyle();

    readonly string SPRITE_IMAGE = "image";
    readonly string NAME = "name";
    readonly string FLAG = "flag";
    readonly string BASIC_CARDS = "basic_cards";
    readonly string GOLDEN_CARDS = "golden_class_specific_cards";

    float space = 20.0f;
    float index_offset = 1;

    int last_basicArr;
    int last_goldenArr;

    private void OnEnable()
    {
        s_object = new SerializedObject(target);
        InitializeProperties();
        InitializeStyle();
        last_basicArr = p_basic_card.arraySize;
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
        GUILayout.Space(space);

        ///// Display Texture
        GUILayout.Label(texture, bold_style);
        ///// Display Class Name
        EditorGUILayout.LabelField(p_name.stringValue, bold_style);

        GUILayout.Space(space);

        ///// Display Properties
        DisplayArr(p_basic_card, ref last_basicArr, "Basic Cards");
        GUILayout.Space(space);

        DisplayArr(p_golden_card, ref last_goldenArr, "Golden Cards");
        GUILayout.Space(space);

        ////// Apply Button
        if (GUILayout.Button(new GUIContent("Apply")))
            Save();
        EditorGUILayout.EndVertical();

        s_object.ApplyModifiedProperties();
    }

    private void DisplayArr(SerializedProperty myArr, ref int lastArrSizeValue, string label)
    {
        lastArrSizeValue = EditorGUILayout.IntField(new GUIContent(label), myArr.arraySize);
        if (lastArrSizeValue != myArr.arraySize)
        {
            if (EditorUtility.DisplayDialog("Confirm", "Do you want to apply your modifications?", "Apply", "Cancel"))
                myArr.arraySize = lastArrSizeValue;
        }
        if (myArr.arraySize != 0)
        {
            for (int i = 0; i < myArr.arraySize; i++)
            {
                string temp = myArr.GetArrayElementAtIndex(i).stringValue;
                EditorGUILayout.TextField(new GUIContent($"Selection {i + index_offset} : "), temp);
            }
        }
    }

    private void InitializeProperties()
    {
        p_image = s_object.FindProperty(SPRITE_IMAGE);
        p_name = s_object.FindProperty(NAME);
        p_flag = s_object.FindProperty(FLAG);
        p_basic_card = s_object.FindProperty(BASIC_CARDS);
        p_golden_card = s_object.FindProperty(GOLDEN_CARDS);
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
