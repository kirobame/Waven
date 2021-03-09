using System;
using Flux.Event;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackChallenge", menuName = "Waven/Challenges/Spell")]
public class SpellChallenge : UseChallenge
{
    public override string GetDescription()
    {
        var subject = goal > 1 ? "sorts" : "sort";
        return $"Utiliser précisément {goal} {subject}.";
    }

    protected override void OnTurnedOn()
    {
        Events.RelayByValue<int>(ChallengeEvent.OnSpellUse, OnAction);
        base.OnTurnedOn();
        
    }
    protected override void OnTurnedOff()
    {
        Events.BreakValueRelay<int>(ChallengeEvent.OnSpellUse, OnAction);
        base.OnTurnedOff();
    }
}