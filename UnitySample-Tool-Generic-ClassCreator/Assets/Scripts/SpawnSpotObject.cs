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

        int log = (int)Mathf.Log(mask, bit_value);
        int pow = (int)Mathf.Pow(bit_value, log);
        if (mask > pow)
            ProcessCombineLayers(mask, pow, monster.abilities);
        else
            ProcessSingleLayer(mask, pow, monster.abilities);
        Debug.Log(monster.abilities.Count);
        return monster;
    }

    private void ProcessCombineLayers(int mask, int pow, List<Ability> abilities)
    {
        try
        {

            int remains = mask - pow;
            if (remains < 0)
                return;

            Type type = Type.GetType(LayerMask.LayerToName(remains + layer_offset));
            if (type != null)
                abilities?.Add(System.Activator.CreateInstance(type) as Ability);

            int newLog = (int)Mathf.Log(remains, bit_value);
            int newPow = (int)Mathf.Pow(bit_value, newLog);
            if (mask > pow)
                ProcessCombineLayers(remains, newPow, abilities);
        }
        catch (System.StackOverflowException e)
        {
            Debug.Log("StackOverflow Exception" + e.Message);
        }
    }

    private void ProcessSingleLayer(int mask, int pow, List<Ability> abilities)
    {
        Type type = Type.GetType(LayerMask.LayerToName((mask - pow) + layer_offset));
        if (type != null)
            abilities?.Add(System.Activator.CreateInstance(type) as Ability);
    }
}
