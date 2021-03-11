using System;
using Flux;
using Flux.Event;
using UnityEngine;

[Serializable]
public class DelayedIntCastArgs : CastArgs, IWrapper<int>
{
    public int Value => state ? value : 0;

    public DelayedIntCastArgs(int value, IMutable target)
    {
        this.value = value;
        this.target = target;
    }

    private IMutable target;
    
    private ITurnbound turnbound;
    private bool isBounded;

    private bool state;
    private int value;
    
    public override CastArgs Copy() => new DelayedIntCastArgs(value, target);
    
    public override void Initialize(IAttributeHolder owner)
    {
        base.Initialize(owner);
        
        isBounded = ((Component)owner).TryGetComponent<ITurnbound>(out turnbound);
        Events.RelayByValue<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        Events.RelayByValue<Turn>(GameEvent.OnTurnEnd, OnTurnEnd);
        
        state = false;
    }
    
    void OnTurnStart(Turn turn)
    {
        if (!isBounded || turn.Target != turnbound || state) return;
        
        if (target == null) End();
        else
        {
            state = true;
            target.Dirty();
        }
    }
    void OnTurnEnd(Turn turn)
    {
        if (!isBounded || turn.Target != turnbound || !state) return;

        End();
    }

    private void End()
    {
        Events.BreakValueRelay<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        Events.BreakValueRelay<Turn>(GameEvent.OnTurnEnd, OnTurnEnd);
        owner.Remove(this);
    }
}