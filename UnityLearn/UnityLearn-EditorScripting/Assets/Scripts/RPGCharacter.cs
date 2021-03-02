using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGCharacter : MonoBehaviour
{
    [Header("Player Informations")]
    [SerializeField] private float health;
    [SerializeField] private float mana;
    [SerializeField] private float stamina;

    [HideInInspector] public int experience;
    private int max_per_level = 750;

    [HideInInspector] public Vector3 target;

    [Header("Spawning objects")]
    [HideInInspector] public GameObject obj;
    [HideInInspector] public Vector3 spawn_point;

    public void BuildObject()
    {
        Instantiate(obj, spawn_point, Quaternion.identity);
    }

    public int LevelUp { get => experience / max_per_level; }
}
