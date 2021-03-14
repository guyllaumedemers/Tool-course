using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

[CustomEditor(typeof(SpawnSpotObject))]
public class SpawnSpotObjectEditor : Editor
{
    SerializedProperty p_spawnspot_selectTypeName;
    SerializedProperty p_abilities_mask;
    SerializedObject spawnspot_obj;
    readonly string TYPE_NAME = "monster_typeName";
    readonly string ABILITIES_MASK = "ability_mask";
    //readonly string TYPE_ENUM = "monsterType_enum";
    string[] names;
    string[] abilities;
    int monsterType_index = -1;
    int mask = -1;

    public void OnEnable()
    {
        BasicInitialization();
        //// Retrieve values from the SpawnSpot Object class => if not initialize it returns null values which leads you to initialize first Time Enable is called
        monsterType_index = GetIndex(names, p_spawnspot_selectTypeName.stringValue);
        //// Nothing == 0, Everything == -1
        mask = p_abilities_mask.intValue;

        if (monsterType_index == -1 && mask == -2)
        {
            monsterType_index = 0;
            mask = 0;
            SetMonsterName(names[monsterType_index]);
            SetAbilityMask(mask);
        }
        else
        {
            SetMonsterName(names[monsterType_index]);
            SetAbilityMask(mask);
        }
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        //// FIRST => Update the Serialize Object
        spawnspot_obj.Update();

        //EditorGUILayout.PropertyField(spawnspot_selectTypeName, new GUIContent("Monster Name")); /// Debugging purposes only
        monsterType_index = EditorGUILayout.Popup(new GUIContent("Monster Type"), monsterType_index, names);

        //// Set the string value after monter type select
        SetMonsterName(names[monsterType_index]);

        //// Draw Abilities Selection
        mask = EditorGUILayout.MaskField(new GUIContent("Abilities"), mask, abilities);
        SetAbilityMask(mask);

        //// LAST => Applies Modifies Properties of the Serialize Object
        spawnspot_obj.ApplyModifiedProperties();
    }

    private void BasicInitialization()
    {
        spawnspot_obj = new SerializedObject(target);
        ///// Retrieve the enum names
        //names = spawnspot_obj.FindProperty(TYPE_ENUM).enumNames; /// Need to be fill from a folder instead

        names = GetFilesNameFromDirectory(Application.dataPath + "/Scripts/_MonsterType/");
        abilities = GetFilesNameFromDirectory(Application.dataPath + "/Scripts/_Abilities/");

        ///// Retrieve the name of the monster from spawnspot object
        p_spawnspot_selectTypeName = spawnspot_obj.FindProperty(TYPE_NAME);

        ///// Retrieve the Abilities[] from the spawnspot Object
        p_abilities_mask = spawnspot_obj.FindProperty(ABILITIES_MASK);
    }

    private void SetMonsterName(string name)
    {
        ((SpawnSpotObject)target).monster_typeName = name;
        ((SpawnSpotObject)target).gameObject.name = $"SpawnSpot {name}";
    }

    private void SetAbilityMask(int mask)
    {
        ((SpawnSpotObject)target).ability_mask = mask;
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

    private string[] GetFilesNameFromDirectory(string path)
    {
        string[] files_name = Directory.GetFiles(path, "*.cs");

        for (int i = 0; i < files_name.Length; i++)
        {
            files_name[i] = Path.GetFileNameWithoutExtension(files_name[i]);
        }
        return files_name;
    }
}
