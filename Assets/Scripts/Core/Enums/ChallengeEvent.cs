using Flux;

[Address]
public enum ChallengeEvent : byte
{
    OnMove,
    OnSpellUse,
    OnAttack,
    OnDamage,
    OnKill,
    OnPush,
    OnBarrierDown
}