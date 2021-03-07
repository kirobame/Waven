using Flux;

[Address]
public enum GameEvent : byte
{
    OnTurnStart,
    OnTurnTimer,
    
    OnTileChange,
    
    OnSpellUsed,
    
    OnPlayerDeath
}