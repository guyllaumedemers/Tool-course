using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    SerializedObject pControllerEditor;
    SerializedProperty pController_bulletType;
    private string BULLET_TYPE = "bulletType";
    private string[] enums;
    private int index = 0;

    private void OnEnable()
    {
        pControllerEditor = new SerializedObject(target);
        pController_bulletType = pControllerEditor.FindProperty(BULLET_TYPE);
        enums = pController_bulletType.enumNames;
    }

    public override void OnInspectorGUI()
    {
        pControllerEditor.Update();

        index = EditorGUILayout.Popup(new GUIContent("Bullet Type"), index, enums);
        PlayerController.Instance.GetBulletType = (BulletType)index;

        pControllerEditor.ApplyModifiedProperties();
    }
}
