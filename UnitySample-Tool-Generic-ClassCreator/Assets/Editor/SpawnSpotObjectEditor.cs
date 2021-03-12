using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(SpawnSpotObject))]
public class SpawnSpotObjectEditor : Editor
{
    SerializedObject spawnspot_obj;
    SerializedProperty spawnspot_selectTypeName;
    readonly string TYPE_NAME = "monster_typeName";
    readonly string TYPE_ENUM = "monsterType_enum";
    string[] names;
    int index = 0;

    public void OnEnable()
    {
        BasicInitialization();
        ///// Set the name of the monster at first initialization to be use in the spawnspot object
        spawnspot_selectTypeName.stringValue = names[2];
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        //// FIRST => Update the Serialize Object
        spawnspot_obj.Update();

        EditorGUILayout.PropertyField(spawnspot_selectTypeName, new GUIContent("Monster Name"));

        index = EditorGUILayout.Popup(new GUIContent("Monster Type"), index, names);
        //spawnspot_selectTypeName.stringValue = names[index];


        //// LAST => Applies Modifies Properties of the Serialize Object
        spawnspot_obj.ApplyModifiedProperties();
    }

    private void BasicInitialization()
    {
        spawnspot_obj = new SerializedObject(target);
        ///// Retrieve the enum names
        names = spawnspot_obj.FindProperty(TYPE_ENUM).enumNames;
        ///// Retrieve the name of the monster from spawnspot object
        spawnspot_selectTypeName = spawnspot_obj.FindProperty(TYPE_NAME);
    }

    private void SetMonsterName(string myName)
    {
        ((SpawnSpotObject)target).monster_typeName = myName;
    }
}
