using System;
using Flux.Event;

[Serializable]
public class AttackChallenge : ToggleChallenge
{
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