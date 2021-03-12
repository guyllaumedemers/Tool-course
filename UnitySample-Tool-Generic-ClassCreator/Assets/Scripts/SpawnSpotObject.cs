using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnSpotObject : MonoBehaviour
{
    public string monster_typeName;
    public MonsterType monsterType_enum;

    private void Awake()
    {
        ///// Check first if the string is null or not, instanciate gameobject => monsters on the screen
        if (!String.IsNullOrEmpty(monster_typeName))
            InstanciateMonster(monster_typeName);
        ///// I want to be able to see onAwake : what is the zone name and the monster type that is spawned on it
    }

    private Monster InstanciateMonster()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = transform.position;

        Monster monster = go.AddComponent<Monster>();
        //// set monster type

        //// set monster name

        return monster;
    }

    private Monster InstanciateMonster(string type)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = transform.position;

        Type monster_type = Type.GetType(type);

        Monster monster = go.AddComponent(monster_type) as Monster;

        return monster;
    }
}
