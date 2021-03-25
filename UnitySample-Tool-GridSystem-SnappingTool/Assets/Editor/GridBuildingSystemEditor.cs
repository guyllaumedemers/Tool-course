using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridController))]
public class GridBuildingSystemEditor : Editor
{
    SerializedObject s_object;
    SerializedProperty p_width;
    SerializedProperty p_height;
    SerializedProperty p_gridObject;

    string PREFABS_PATH = "Prefabs/";
    GridObject[] gridObjects;

    int minSlider = 1;
    int maxSlider = 10;

    private void OnEnable()
    {
        s_object = new SerializedObject(target);
        InitializeAllProperties();
        gridObjects = Resources.LoadAll<GridObject>(PREFABS_PATH);
    }

    public override void OnInspectorGUI()
    {
        s_object.Update();

        /////// create slider for width
        p_width.intValue = EditorGUILayout.IntSlider(new GUIContent("Grid Width"), p_width.intValue, minSlider, maxSlider);
        ((GridController)target).width = p_width.intValue;
        /////// create slider for height
        p_height.intValue = EditorGUILayout.IntSlider(new GUIContent("Grid Height"), p_height.intValue, minSlider, maxSlider);
        ((GridController)target).height = p_height.intValue;

        GUILayout.Space(20f);

        EditorGUILayout.LabelField("Buildings Selection");
        EditorGUILayout.BeginHorizontal();
        int cpt = 0;
        foreach (GridObject gridObject in gridObjects)
        {
            ++cpt;
            if (cpt > 2)
                EditorGUILayout.EndHorizontal();
            Texture preview = AssetPreview.GetAssetPreview(gridObject.gameObject);
            if (GUILayout.Button(preview))
                ((GridController)target).gridObject = gridObject;
        }

        s_object.ApplyModifiedProperties();
    }

    private void InitializeAllProperties()
    {
        p_width = s_object.FindProperty("width");
        p_height = s_object.FindProperty("height");
        p_gridObject = s_object.FindProperty("gridObject");
    }
}
