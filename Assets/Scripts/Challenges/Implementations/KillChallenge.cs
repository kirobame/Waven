using System;
using Flux.Event;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackChallenge", menuName = "Waven/Challenges/Kill")]
public class KillChallenge : ToggleChallenge
{
    public override string GetDescription()
    {
        if (execute) return "Détruire au moins une entité.";
        else return "Ne détruire aucune entité.";
    }

    protected override void OnTurnedOn()
    {
        Events.Register(ChallengeEvent.OnKill, OnAction);
        base.OnTurnedOn();
    }
    protected override void OnTurnedOff()
    {
        Events.Unregister(ChallengeEvent.OnKill, OnAction);
        base.OnTurnedOff();
    }
}