using Flux.Event;
using System;
using UnityEngine;

[Serializable]
public abstract class TemporaryCastArgs : CastArgs
{
    public override void Initialize(IAttributeHolder owner)
    {
        base.Initialize(owner);
        Events.RelayByVoid(GameEvent.OnTurnStart, OnTurnEnd);
    }

    void OnTurnEnd()
    {
        Debug.Log($"REMOVING : {this}");
        
        Events.BreakVoidRelay(GameEvent.OnTurnStart, OnTurnEnd);
        owner.Remove(this);
    }
}