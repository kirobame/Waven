using System;
using Flux.Event;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackChallenge", menuName = "Waven/Challenges/Attack")]
public class AttackChallenge : ToggleChallenge
{
    public override string GetDescription()
    {
        var prefix = execute ? "Utiliser" : "Ne pas utiliser";
        return $"{prefix} l'attaque simple.";
    }

    protected override void OnTurnedOn()
    {
        Events.Register(ChallengeEvent.OnAttack, OnAction);
        base.OnTurnedOn();
        
    }
    protected override void OnTurnedOff()
    {
        Events.Unregister(ChallengeEvent.OnAttack, OnAction);
        base.OnTurnedOff();
    }
}