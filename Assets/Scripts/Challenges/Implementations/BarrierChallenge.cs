using Flux.Event;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBarrierChallenge", menuName = "Waven/Challenges/Barrier")]
public class BarrierChallenge : ToggleChallenge
{
    public override string GetDescription()
    {
        if (execute) return "Endommager au moins une barrière.";
        else return "N'endommager aucune barrière.";
    }

    protected override void OnTurnedOn()
    {
        Events.Register(ChallengeEvent.OnBarrierDown, OnAction);
        base.OnTurnedOn();
    }
    protected override void OnTurnedOff()
    {
        Events.Unregister(ChallengeEvent.OnBarrierDown, OnAction);
        base.OnTurnedOff();
    }
}