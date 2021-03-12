﻿using Flux;
using System.Collections.Generic;
using System.Linq;
using Flux.Event;
using Flux.Data;
using Flux.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

public class Trap : TileableBase
{
    [SerializeField] protected Spell spell;
    [SerializeField] private AudioClipPackage trapSound;
    [SerializeField] protected Animator animator;
    
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
        Apply(source);
    }

    protected virtual void Apply(ITileable source)
    {
        var map = Repository.Get<Map>(References.Map);
        var tile = map.Tilemap.WorldToCell(transform.position).ToTile();
        
        if (tile.Entities.Any(tileable => tileable is Tileable)) animator.SetTrigger("Explode");

        Routines.Start(Routines.DoAfter(() =>
        {
            spell.Prepare();
            spell.CastFrom(tile, Spellcaster.EmptyArgs);

        }, new YieldTime()));
    }
}