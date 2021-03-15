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
    readonly int bit_value = 2;

    private void Awake()
    {
        ///// Check first if the string is null or not, instanciate gameobject => monsters on the screen
        if (!String.IsNullOrEmpty(monster_typeName) && ability_mask != -2)
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

        GetLayers(mask, monster.abilities);
        return monster;
    }

    private void GetLayers(int mask, List<Ability> abilities)
    {
        try
        {
            int log = (int)Mathf.Log(mask, bit_value);
            int pow = (int)Mathf.Pow(bit_value, log);

            RecursiveSearch(mask, log, pow, abilities);
        }
        catch (System.StackOverflowException e)
        {
            Debug.Log("StackOverflow Exception" + e.Message);
        }
    }

    private void RecursiveSearch(int mask, int log, int pow, List<Ability> abilities)
    {
        int newMask = mask - (mask - pow);
        int remains = mask - pow;
        if (mask < 0)
            return;
        else if (mask == pow)
            abilities?.Add(System.Activator.CreateInstance(GetLayerType(log)) as Ability);
        else
            Search(newMask, remains, abilities);
    }

    private Type GetLayerType(int log)
    {
        Type type = Type.GetType(LayerMask.LayerToName(log + layer_offset));
        return type;
    }

    private void Search(int newMask, int remains, List<Ability> abilities)
    {
        int log = (int)Mathf.Log(newMask, bit_value);
        int pow = (int)Mathf.Pow(bit_value, log);

        int remains_log = (int)Mathf.Log(remains, bit_value);
        int remains_pow = (int)Mathf.Pow(bit_value, remains_log);

        if (newMask == pow)
            abilities?.Add(System.Activator.CreateInstance(GetLayerType(log)) as Ability);
        if (remains > 0)
            RecursiveSearch(remains, remains_log, remains_pow, abilities);
    }
}
