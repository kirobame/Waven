using Flux;
using Flux.Event;

public class Trap : TileableBase
{
    void Awake() => Events.RelayByValue<ITileable>(GameEvent.OnTileChange, OnTileChange);
    void Start()
    {
        Routines.Start(Routines.DoAfter(() =>
        {
            foreach (var entity in navigator.Current.Entities) ApplyOn(entity);
            
        }, new YieldFrame()));
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Events.BreakValueRelay<ITileable>(GameEvent.OnTileChange, OnTileChange);
    }

    //------------------------------------------------------------------------------------------------------------------/

    void OnTileChange(ITileable source)
    {
        if (source.Navigator.Current != Navigator.Current) return;
        ApplyOn(source);
    }

    private void ApplyOn(ITileable source)
    {
        if (source is Tileable tileable) tileable.InterruptMove();
        if (source.TryGet<IDamageable>(out var damageable)) damageable.Inflict(1, DamageType.Base);
    }
}