using Flux;

[Address]
public enum GameEvent : byte
{
    OnTurnStart,
    OnTurnTimer,
    OnTurnEnd,
    
    OnRewardStart,
    OnRewardTimer,
    OnRewardEnd,

    OnTileChange,
    
    OnBaseAttack,
    OnSpellUsed,
    OnDamageTaken,

    OnPlayerDeath
}