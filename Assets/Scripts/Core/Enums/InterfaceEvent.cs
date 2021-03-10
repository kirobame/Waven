using Flux;

[Address]
public enum InterfaceEvent : byte
{
    OnTooltipUsed,
    
    OnSpellSelected,
    OnSpellTilesAffect,
    OnSpellEnd,

    OnHoverStart,
    OnHoverEnd,
    
    OnInfoRefresh,
    
    OnChallengeUpdate,
    OnChallengeCompleted,
    OnChallengeFailed
}