using System;
using Flux.Event;

[Serializable]
public class KillChallenge : ToggleChallenge
{
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