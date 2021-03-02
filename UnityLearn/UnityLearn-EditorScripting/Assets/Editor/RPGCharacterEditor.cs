using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RPGCharacter))]
public class RPGCharacterEditor : Editor
{
    SerializedProperty m_GameObjectProp;
    private readonly string GAMEOBJECT_NAME = "obj";

    public void OnEnable()
    {
        m_GameObjectProp = serializedObject.FindProperty(GAMEOBJECT_NAME);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Update object/ properties values
        DrawDefaultInspector();

        // Leveling
        RPGCharacter rPGCharacter = target as RPGCharacter;

        rPGCharacter.experience = EditorGUILayout.IntField("Experience", rPGCharacter.experience);
        EditorGUILayout.LabelField("Level", rPGCharacter.LevelUp.ToString());

        // Target
        GUILayout.Label("Target");
        rPGCharacter.target = EditorGUILayout.Vector3Field(" ", rPGCharacter.target);

        // Space
        GUILayout.Space(10.0f);

        // GameObject PropertyField
        EditorGUILayout.PropertyField(m_GameObjectProp, new GUIContent("GameObject"));
        // SpawnPoint for the GameObject
        rPGCharacter.spawn_point = EditorGUILayout.Vector3Field("SpawnPoint", rPGCharacter.spawn_point);

        // Helpbox
        EditorGUILayout.HelpBox("HELP!", MessageType.Info, true);

        // Button => Instanciate a new gameobject
        if (GUILayout.Button($"Build {rPGCharacter.obj.name}"))
            rPGCharacter.BuildObject();

        serializedObject.ApplyModifiedProperties(); // applies object/ properties modifies values
    }
}
