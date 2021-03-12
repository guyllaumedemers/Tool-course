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
    int current_index = -1;

    public void OnEnable()
    {
        BasicInitialization();
        current_index = GetIndex(names, spawnspot_selectTypeName.stringValue);
        if (current_index == -1)
        {
            current_index = 0;
            SetMonsterName(names[current_index]);
        }
        else
        {
            SetMonsterName(names[current_index]);
        }
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        //// FIRST => Update the Serialize Object
        spawnspot_obj.Update();

        //EditorGUILayout.PropertyField(spawnspot_selectTypeName, new GUIContent("Monster Name")); /// Debugging purposes only
        current_index = EditorGUILayout.Popup(new GUIContent("Monster Type"), current_index, names);

        //// Set the string value after monter type select
        SetMonsterName(names[current_index]);

        //// LAST => Applies Modifies Properties of the Serialize Object
        spawnspot_obj.ApplyModifiedProperties();
    }

    private void BasicInitialization()
    {
        spawnspot_obj = new SerializedObject(target);
        ///// Retrieve the enum names
        //names = spawnspot_obj.FindProperty(TYPE_ENUM).enumNames; /// Need to be fill from a folder instead

        names = GetFilesFromDirectory(Application.dataPath + "/Scripts/_MonsterType/");

        ///// Retrieve the name of the monster from spawnspot object
        spawnspot_selectTypeName = spawnspot_obj.FindProperty(TYPE_NAME);
    }

    private void SetMonsterName(string name)
    {
        ((SpawnSpotObject)target).monster_typeName = name;
        ((SpawnSpotObject)target).gameObject.name = $"SpawnSpot {name}";
    }

    private int GetIndex(string[] names, string name)
    {
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].Equals(name))
                return i;
        }
        return -1;
    }

    private string[] GetFilesFromDirectory(string path)
    {
        names = Directory.GetFiles(path, "*.cs");

        for (int i = 0; i < names.Length; i++)
        {
            names[i] = Path.GetFileNameWithoutExtension(names[i]);
        }
        return names;
    }
}
