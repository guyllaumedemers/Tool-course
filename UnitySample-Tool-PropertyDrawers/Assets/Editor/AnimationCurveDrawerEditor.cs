using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AnimationCurveDrawer))]
public class AnimationCurveDrawerEditor : PropertyDrawer
{
    private readonly string ANIMATION_CURVE_STRING = "animationCurve";
    private readonly string CUSTOM_VECTOR2_X = "xRange";
    private readonly string CUSTOM_VECTOR2_Y = "yRange";

    private readonly float xMin = 0;
    private readonly float xMax = 10;
    private readonly float yMin = 0;
    private readonly float yMax = 10;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUIUtility.labelWidth = 50;

        SerializedProperty p_curve = property.FindPropertyRelative(ANIMATION_CURVE_STRING);
        SerializedProperty p_xRange = property.FindPropertyRelative(CUSTOM_VECTOR2_X);
        SerializedProperty p_yRange = property.FindPropertyRelative(CUSTOM_VECTOR2_Y);

        EditorGUI.BeginProperty(position, label, property);
        Rect[,] rects = CompactSingleRow.GetGrid(1, 3, position.position, position.width, position.height);

        p_curve.animationCurveValue = EditorGUI.CurveField(rects[0, 0], new GUIContent("Anim V2"), p_curve.animationCurveValue);

        float[] xRange = CompactSingleRow.UnpackVector2(p_xRange.vector2Value);
        float[] yRange = CompactSingleRow.UnpackVector2(p_yRange.vector2Value);

        Rect compact_xRange = rects[0, 1];
        Rect compact_yRange = rects[0, 2];
        Rect[] xRangeRect = CompactSingleRow.GetSubdivisionWithPrefixe(compact_xRange);
        Rect[] yRangeRect = CompactSingleRow.GetSubdivisionWithPrefixe(compact_yRange);

        EditorGUI.LabelField(xRangeRect[0], new GUIContent("xR"));
        float xMin = EditorGUI.FloatField(xRangeRect[1], p_xRange.vector2Value.x);
        float xMax = EditorGUI.FloatField(xRangeRect[2], p_xRange.vector2Value.y);

        p_xRange.vector2Value = new Vector2(xMin, xMax);

        EditorGUI.LabelField(yRangeRect[0], new GUIContent("yR"));
        float yMin = EditorGUI.FloatField(yRangeRect[1], p_yRange.vector2Value.x);
        float yMax = EditorGUI.FloatField(yRangeRect[2], p_yRange.vector2Value.y);

        p_yRange.vector2Value = new Vector2(yMin, yMax);

        //EditorGUI.MultiFloatField(rects[0, 1], new GUIContent(""), new GUIContent[] { new GUIContent("xMin"), new GUIContent("xMax") }, xRange);
        //EditorGUI.MultiFloatField(rects[0, 2], new GUIContent(""), new GUIContent[] { new GUIContent("yMin"), new GUIContent("yMax") }, yRange);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
