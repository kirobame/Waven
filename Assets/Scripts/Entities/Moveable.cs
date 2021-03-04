using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Flux;
using Flux.Data;
using Flux.Event;
using Sirenix.Utilities;
/*
public interface IMoveable
{
    int movementPoints { get; }
    int actualMovementPoints { get; }

}*/

public abstract class Moveable : Navigator//, IMoveable
{
    [SerializeField, Range(0, 5)] int movementPoints;
    public int actualMovementPoints { get; private set; }
    List<int> movementPointsBoosts = new List<int>();

    public void BoostPM(int boostValue)
    {
        movementPointsBoosts.Add(boostValue);
        actualMovementPoints += boostValue;
    }
    public void UnBoostPM(int boostValue)
    {
        movementPointsBoosts.Remove(boostValue);
        actualMovementPoints -= boostValue;
    }


}