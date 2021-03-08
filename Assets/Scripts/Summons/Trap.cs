using Flux;
using System.Collections.Generic;
using Flux.Event;
using Flux.Data;
using Sirenix.OdinInspector;
using UnityEngine;

public class Trap : TileableBase
{
    [SerializeField] private Spell spell;

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
        Apply();
    }

    protected virtual void Apply()
    {
        var map = Repository.Get<Map>(References.Map);
        var tile = map.Tilemap.WorldToCell(transform.position).ToTile();
        
        spell.Prepare();
        spell.CastFrom(tile, Spellcaster.EmptyArgs);
    }
}
