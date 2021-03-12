using Flux;

[Address]
public enum InterfaceEvent : byte
{
    OnTooltipUsed,
    OnHideTooltip,
    
    OnSpellSelected,
    OnSpellTilesAffect,
    OnSpellInterruption,
    OnSpellEnd,

    OnHoverStart,
    OnHoverEnd,
    
    OnInfoRefresh,
    
    OnChallengeUpdate,
    OnChallengeCompleted,
    OnChallengeFailed
}