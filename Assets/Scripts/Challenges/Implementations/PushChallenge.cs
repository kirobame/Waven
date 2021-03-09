using System;
using Flux;
using Flux.Event;
using UnityEngine;

[Serializable]
public class PushChallenge : ToggleChallenge
{
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