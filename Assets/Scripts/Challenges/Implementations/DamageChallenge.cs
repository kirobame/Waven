using System;
using Flux;
using Flux.Event;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackChallenge", menuName = "Waven/Challenges/Damage")]
public class DamageChallenge : ToggleChallenge
{
    public override string GetDescription()
    {
        if (execute) return "Causer n'importe quel montant de dommage.";
        else return "Ne causer aucun dommage.";
    }
    
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
        if (!(args is IWrapper<Damageable> wrapper)) return;
        
        if (!wrapper.Value.IsAlive || !wrapper.Value.TryGetComponent<Player>(out var player) || player == Player.Active) return;
        base.OnAction(args);
    }
}