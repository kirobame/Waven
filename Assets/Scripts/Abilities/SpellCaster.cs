using System;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    Spell selectedSpell;
    object caster;

    void Start()
    {
        Events.Open(SpellEvents.OnSpellSelected);
        Events.Open(SpellEvents.OnSpellCasted);

        Events.RelayByValue<Spell>(SpellEvents.OnSpellSelected, PrepareCast);
    }

    bool CanBeCasted(uint casterMana)
    {
        if (casterMana >= selectedSpell.Cost())
            return true;
        else
            return false;
    }

    void PrepareCast(Spell _selectedSpell)
    {
        caster = default;                   //get current caster
        selectedSpell = _selectedSpell;
        uint casterMana = 10;                // -> caster.mana

        if (!CanBeCasted(casterMana))
        {
            Debug.Log("Spell " + selectedSpell.name + " Can't be casted");
            Events.ZipCall<bool>(SpellEvents.OnSpellCasted, false);
            return;
        }
        Debug.Log("Spell " + selectedSpell.name + " Can be casted");


        Events.BreakValueRelay<Spell>(SpellEvents.OnSpellSelected, PrepareCast);
        Events.RelayByValue<Vector3Int>(SelectionEvents.OnTileSelected, Cast);
        Debug.Log("Waiting for cible");
    }

    void Cast(Vector3Int selectedTile)
    {
        Debug.Log("Spell " + selectedSpell.name + " is casting");
        List<Vector3Int> tiles = new List<Vector3Int>();
        Events.BreakValueRelay<Vector3Int>(SelectionEvents.OnTileSelected, Cast);

        //Get affected tiles -> CastPos
        
        foreach (var effect in selectedSpell.Effects())
        {
            Debug.Log("Effect " + effect + "launched");
            effect.PlayEffect(tiles);
        }
        
        //caster.mana -= spell.cost
        Casted();
    }

    void Casted()
    {
        Debug.Log(selectedSpell.name + " casted");
        Events.RelayByValue<Spell>(SpellEvents.OnSpellSelected, PrepareCast);
        Events.ZipCall<bool>(SpellEvents.OnSpellCasted, true);
    }
}
