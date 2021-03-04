﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICaster
{
    int force { get; }
    int actualForce { get; }
    int attack { get; }
    int actualAttack { get; }

    List<Spell> spellDeck { get; }
}
public abstract class Caster : MonoBehaviour, ICaster
{
    public int force { get; }
    public int actualForce { get; private set; }
    List<int> forceBoosts = new List<int>();
    public void BoostForce(int boostValue)
    {
        forceBoosts.Add(boostValue);
        actualForce += boostValue;
    }
    public void UnBoostForce(int boostValue)
    {
        forceBoosts.Remove(boostValue);
        actualForce -= boostValue;
    }

    public int attack { get; }
    public int actualAttack { get; private set; }
    List<int> attackBoosts = new List<int>();
    public void BoostAttack(int boostValue)
    {
        attackBoosts.Add(boostValue);
        actualAttack += boostValue;
    }
    public void UnBoostAttack(int boostValue)
    {
        attackBoosts.Remove(boostValue);
        actualAttack -= boostValue;
    }


    public List<Spell> spellDeck { get; private set; } = new List<Spell>();
}
