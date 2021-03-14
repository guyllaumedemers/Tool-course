using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnSpotObject : MonoBehaviour
{
    public string monster_typeName;
    //public MonsterType monsterType_enum;
    public int ability_mask;
    readonly int layer_offset = 6;

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
        //Debug.Log($"Log value : {(int)Mathf.Log(mask, 2)}");
        Debug.Log($"Mask Name : {LayerMask.LayerToName((int)Mathf.Log(mask, 2) + layer_offset)}");
        //Debug.Log($"Pow value : {Math.Pow(log, 2)}");
        int log = (int)Mathf.Log(mask, 2);
        int pow = (int)Math.Pow(log, 2);

        ///// So we retrieve the mask value of the combine layer => example : mask = 12, log2(12) = (int)3.5 = 3, Pow(3,2) = 9
        ///// mask > 9 => so its a combine layer
        ///// Lets test this with another example to confirm the theory
        ///// example 2 : mask = 6, log2(6) = (int)2.5 = 2, Pow(2,2) = 4 which in our case is already use and gives lazerbeam ability
        ///// example 3 values that are not next to each other => mask = 10, log2(10) = (int)3.3 = 3, Pow(3,2) = 9
        if (mask > pow)
            ProcessCombineLayers();

        return monster;
    }

    private void ProcessCombineLayers()
    {
        ///// How do we process them?
        ///// Mask values are binary based => 2^0, 2^1, 2^2....
        ///// mask - pow = mask position(big value) => from our last example its 3, which gives 2^3 = 8
        ///// mask - mask position = what is left => 12 -8 = 4 => log2(4) = 2, which is the remainding position we are looking for
    }
}
