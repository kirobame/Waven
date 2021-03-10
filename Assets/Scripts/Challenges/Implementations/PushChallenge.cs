using System;
using Flux;
using Flux.Event;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackChallenge", menuName = "Waven/Challenges/Push")]
public class PushChallenge : ToggleChallenge
{
    public override string GetDescription()
    {
        if (execute) return "Changer de place au moins une entité par n'importe quel moyen.";
        else return "Ne changer de place aucune entité sur le terrain.";
    }

    protected override void OnTurnedOn()
    {
        Events.Register(ChallengeEvent.OnPush, OnAction);
        base.OnTurnedOn();
    }
    protected override void OnTurnedOff()
    {
        Events.Unregister(ChallengeEvent.OnPush, OnAction);
        base.OnTurnedOff();
    }
}