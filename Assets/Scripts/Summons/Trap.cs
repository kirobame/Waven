using Flux;
using System.Collections.Generic;
using Flux.Event;
using Flux.Data;
using Sirenix.OdinInspector;
using UnityEngine;

public class Trap : TileableBase
{
    [SerializeField] private List<Effect> effects = new List<Effect>();

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
        //Debug.Log("OnTileChange");

        if (source.Navigator.Current != Navigator.Current) return;
        ApplyOn(source);
    }

    protected virtual void ApplyOn(ITileable source)
    {
        var map = Repository.Get<Map>(References.Map);
        Tile tile = map.Tilemap.WorldToCell(transform.position).ToTile();
        
        foreach (var effect in effects)
        {
            effect.PlayOn(tile, Spellcaster.EmptyArgs);
        }
    }
}
