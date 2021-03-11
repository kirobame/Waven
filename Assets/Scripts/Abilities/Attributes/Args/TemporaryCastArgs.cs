using Flux.Event;
using System;
using Flux.Data;
using UnityEngine;

[Serializable]
public abstract class TemporaryCastArgs : CastArgs
{
    public TemporaryCastArgs(int duration, bool self)
    {
        this.duration = duration;
        this.self = self;
    }

    [SerializeField] protected int duration;
    [SerializeField] protected bool self;
    
    private int counter;
    private bool isBounded;
    private ITurnbound turnbound;
    
    public override void Initialize(float time, IAttributeHolder owner)
    {
        base.Initialize(time, owner);

        isBounded = true;
        
        if (self) turnbound = Player.Active;
        else
        {
            var index = Player.Active.Index;
            var opposite = index == 0 ? 1 : 0;
            turnbound = Repository.GetAt<Player>(References.Players, opposite);
        }
       
        
        counter = duration;
        Events.RelayByValue<Turn>(GameEvent.OnTurnEnd, OnTurnEnd);
    }

    void OnTurnEnd(Turn turn)
    {
        if (isBounded)
        {
            if (turn.Target == turnbound)
            {
                counter--;
                if (counter > 0) return;
            }
            else return;
        }
        else return;
        
        Events.BreakValueRelay<Turn>(GameEvent.OnTurnEnd, OnTurnEnd);
        owner.Remove(this);
    }
}