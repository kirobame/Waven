﻿using Flux.Event;
using System;
using UnityEngine;

[Serializable]
public abstract class TemporaryCastArgs : CastArgs
{
    [SerializeField] private int duration;
    
    private int counter;
    private bool isBounded;
    private ITurnbound turnbound;
    
    public override void Initialize(IAttributeHolder owner)
    {
        base.Initialize(owner);

        isBounded = ((Component)owner).TryGetComponent<ITurnbound>(out turnbound);
        
        counter = duration;
        Events.RelayByValue<Turn>(GameEvent.OnTurnStart, OnTurnEnd);
    }

    void OnTurnEnd(Turn turn)
    {
        if (isBounded && turn.Target == turnbound)
        {
            counter--;
            if (counter > 0) return;
        }
        
        Events.BreakValueRelay<Turn>(GameEvent.OnTurnStart, OnTurnEnd);
        owner.Remove(this);
    }
}