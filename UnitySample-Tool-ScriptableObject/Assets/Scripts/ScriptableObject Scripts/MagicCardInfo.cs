using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/New Spell")]
public class MagicCardInfo : ScriptableObject
{
    public bool flag;
    public Sprite image;
    public new string name;
    public string description;
    public int cost;
}
