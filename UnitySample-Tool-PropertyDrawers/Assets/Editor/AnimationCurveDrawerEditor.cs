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

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUIUtility.labelWidth = 50;

        SerializedProperty p_curve = property.FindPropertyRelative(ANIMATION_CURVE_STRING);
        SerializedProperty p_xRange = property.FindPropertyRelative(CUSTOM_VECTOR2_X);
        SerializedProperty p_yRange = property.FindPropertyRelative(CUSTOM_VECTOR2_Y);

        EditorGUI.BeginProperty(position, label, property);


        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
