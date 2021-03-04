using Flux.Event;
using UnityEngine;

public class Trap : TileableBase
{
    void Awake() => Events.RelayByValue<ITileable>(GameEvent.OnTileChange, OnTileChange);
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Events.BreakValueRelay<ITileable>(GameEvent.OnTileChange, OnTileChange);
    }

    //------------------------------------------------------------------------------------------------------------------/

    void OnTileChange(ITileable source)
    {
        if (source.Navigator.Current != Navigator.Current) return;
        
        if (source is Tileable tileable) tileable.InterruptMove();
        if (source.TryGet<IDamageable>(out var damageable)) damageable.Inflict(1, DamageType.Base);
    }
}