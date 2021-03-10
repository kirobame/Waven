using Flux.Event;

public class BarrierDamageable : Damageable
{
    protected override void OnDeath()
    {
        Events.EmptyCall(ChallengeEvent.OnBarrierDown);
        base.OnDeath();
    }
}