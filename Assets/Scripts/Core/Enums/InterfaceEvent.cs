using Flux;

[Address]
public enum InterfaceEvent : byte
{
    OnTooltipUsed,
    
    OnSpellSelected,

    OnHoverStart,
    OnHoverEnd,
    
    OnInfoRefresh,
}