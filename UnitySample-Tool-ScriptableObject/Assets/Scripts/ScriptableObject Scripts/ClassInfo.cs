using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Class", menuName = "Class/New Class")]
public class ClassInfo : ScriptableObject
{
    public bool flag;
    public Sprite image;
    public new string name;
    public string[] basic_cards;
    public string[] golden_class_specific_cards;
}
