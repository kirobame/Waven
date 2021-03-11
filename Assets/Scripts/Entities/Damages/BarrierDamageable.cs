using Flux.Event;
using UnityEngine;

public class BarrierDamageable : Damageable
{
    [SerializeField] private new SpriteRenderer renderer;
    [SerializeField] private Sprite damagedVersion;

    protected override void OnLogicDone()
    {
        Events.EmptyCall(ChallengeEvent.OnBarrierDown);
        
        var life = Get("Health");
        if (life.actualValue == 1) renderer.sprite = damagedVersion;
        
        base.OnLogicDone();
    }

    protected override void OnDeath()
    {
        Events.EmptyCall(ChallengeEvent.OnBarrierDown);
        base.OnDeath();
    }
}