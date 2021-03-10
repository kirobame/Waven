using System;
using Flux.Event;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackChallenge", menuName = "Waven/Challenges/Movement")]
public class MovementChallenge : UseChallenge
{
    public override string GetDescription()
    {
        var subject = goal > 1 ? "cases" : "case";
        return $"Se déplacer précisément de {goal} {subject}.";
    }

    protected override void OnTurnedOn()
    {
        base.OnTurnedOn();
        Events.RelayByValue<int>(ChallengeEvent.OnMove, OnAction);
    }
    protected override void OnTurnedOff()
    {
        Events.BreakValueRelay<int>(ChallengeEvent.OnMove, OnAction);
        base.OnTurnedOff();
    }
}