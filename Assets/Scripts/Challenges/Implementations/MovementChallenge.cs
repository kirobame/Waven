using System;
using Flux.Event;

[Serializable]
public class MovementChallenge : UseChallenge
{
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