using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Minions", menuName = "Minions/New Minion")]
public class MinionInfo : ScriptableObject
{
    public bool flag;
    public Sprite image;
    public new string name;
    public string description;
    public int health;
    public int defense;
    public int cost;
}
