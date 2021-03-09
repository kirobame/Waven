using System;
using Flux;
using Flux.Event;
using UnityEngine;

[Serializable]
public class DamageChallenge : ToggleChallenge
{
    protected override void OnTurnedOn()
    {
        Events.Register(ChallengeEvent.OnDamage, OnAction);
        base.OnTurnedOn();
        
    }
    protected override void OnTurnedOff()
    {
        Events.Unregister(ChallengeEvent.OnDamage, OnAction);
        base.OnTurnedOff();
    }

    protected override void OnAction(EventArgs args)
    {
        if (!(args is IWrapper<Damageable> damageable)) return;
        
        if (!damageable.Value.IsAlive || !((Component) damageable).TryGetComponent<Player>(out var player) || player == Player.Active) return;
        base.OnAction(args);
    }
}