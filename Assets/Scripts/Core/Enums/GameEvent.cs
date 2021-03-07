using Flux;

[Address]
public enum GameEvent : byte
{
    OnTurnStart,
    OnTurnTimer,
    
    OnTileChange,
    
    OnBaseAttack,
    OnSpellUsed,
    OnDamageTaken,

    OnPlayerDeath
}