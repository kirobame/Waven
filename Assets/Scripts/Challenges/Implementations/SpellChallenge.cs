using System;
using Flux.Event;

[Serializable]
public class SpellChallenge : UseChallenge
{
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