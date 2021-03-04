﻿using Flux;
using Flux.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesHealthBar : MonoBehaviour
{
    
    void Start()
    {
        Events.Open(InterfaceEvent.OnSpellCast);
        Events.RelayByValue<Spell, Tile, HashSet<Tile>>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
        Events.RelayByValue<Spell>(InterfaceEvent.OnSpellCast, OnSpellCast);
    }

    private void OnSpellSelected(Spell actualSpell, Tile source, HashSet<Tile> castZone)
    {
        foreach(var tile in castZone)
        {
            if (tile.IsFree()) continue;

            foreach(var entity in tile.Entities)
            {

            }
        }
    }

    private void OnSpellCast(Spell acutalSpell)
    {
        throw new NotImplementedException();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
