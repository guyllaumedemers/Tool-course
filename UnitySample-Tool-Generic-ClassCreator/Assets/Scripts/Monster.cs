using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public List<Ability> abilities;

    private void Awake()
    {
        abilities = new List<Ability>();
    }

    private void Start()
    {
        if (abilities != null)
            CastSpell();
    }

    private void CastSpell()
    {
        foreach (Ability a in abilities)
        {
            a.CastAbility();
        }
    }
}