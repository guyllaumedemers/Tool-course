using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardAssetInfo))]
public class CardAssetInfoEditor : Editor
{
    /// <summary>
    /// Accesscer Requiered
    /// </summary>
    SerializedObject info_object;
    SerializedProperty p_assetName;
    SerializedProperty p_assetImg;
    SerializedProperty p_assetDescription;

    /// <summary>
    /// Path Strings
    /// </summary>
    readonly string PATH_NAME = "name";
    readonly string PATH_SPRITE = "sprite";
    readonly string PATH_DESCRIPTION = "textDesc";

    private void OnEnable()
    {
        info_object = new SerializedObject(target);
        p_assetName = info_object.FindProperty(PATH_NAME);
        p_assetImg = info_object.FindProperty(PATH_SPRITE);
        p_assetDescription = info_object.FindProperty(PATH_DESCRIPTION);
    }

    public override void OnInspectorGUI()
    {
        info_object.Update();

        EditorGUILayout.PropertyField(p_assetName, new GUIContent("Name"));
        EditorGUILayout.ObjectField(p_assetImg, new GUIContent("Sprite"));

        GUILayout.Label("Character Attributes");
        p_assetDescription.stringValue = EditorGUILayout.TextArea(p_assetDescription.stringValue, GUILayout.Height(100), GUILayout.ExpandWidth(true));

        info_object.ApplyModifiedProperties();
    }
}
