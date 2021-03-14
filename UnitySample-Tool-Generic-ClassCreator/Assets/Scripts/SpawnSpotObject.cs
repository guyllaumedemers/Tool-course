using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnSpotObject : MonoBehaviour
{
    public string monster_typeName;
    //public MonsterType monsterType_enum;
    public int ability_mask;
    readonly int layer_offset = 5;

    private void Awake()
    {
        ///// Check first if the string is null or not, instanciate gameobject => monsters on the screen
        if (!String.IsNullOrEmpty(monster_typeName) && ability_mask != -1)
            InstanciateMonster(monster_typeName, ability_mask);
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

    private Monster InstanciateMonster(string type, int mask)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = transform.position;

        Type monster_type = Type.GetType(type);

        Monster monster = go.AddComponent(monster_type) as Monster;
        ///// Need to retrieve the layers that are set from the Editor GUI to set the abilities of the monster
        ///// PROBLEM => If we do a mixed selection, our layer int value result might also be the value of another layer int
        ///// HOW do we go from mixted values to a stringArr?
        Debug.Log($"Mask int : {mask}");
        Debug.Log($"Layer Name {LayerMask.LayerToName(mask + layer_offset)}");

        return monster;
    }
}
