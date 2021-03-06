using Flux;
using System.Collections.Generic;
using Flux.Event;
using UnityEngine;

public class Trap : TileableBase
{
    [SerializeField] private List<Effect> effects = new List<Effect>();
    [SerializeField] private List<Vector2> vec = new List<Vector2>();

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

    protected virtual void ApplyOn(ITileable source)
    {
        if (source.TryGet<IDamageable>(out var damageable)) damageable.Inflict(1, DamageType.Base);
    }
}