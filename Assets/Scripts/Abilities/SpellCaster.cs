using System;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    Spell selectedSpell;
    List<Effect> remainingEffects = new List<Effect>();
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

        remainingEffects = selectedSpell.Effects();

        Events.BreakValueRelay<Spell>(SpellEvents.OnSpellSelected, PrepareCast);
        Events.RelayByValue<Tile>(SelectionEvents.OnTileSelected, Cast);
        Debug.Log("Waiting for cible");
    }

    void Cast(Tile selectedTile)
    {
        Events.BreakValueRelay<Tile>(SelectionEvents.OnTileSelected, Cast);
        Debug.Log("Spell " + selectedSpell.name + " is casting");
        
        foreach (var effect in remainingEffects)
        {
            if (effect.NeedNewSelection())
                SubCast(effect.GetAffectedTiles(selectedTile));

            Debug.Log("Effect " + effect + "launched");
            effect.LaunchEffect(selectedTile);
            remainingEffects.Remove(effect);
        }
        
        //caster.mana -= spell.cost
        Casted();
    }

    void SubCast (List<Tile> tiles)
    {
        Events.RelayByValue<Tile>(SelectionEvents.OnTileSelected, Cast);
    }

    void Casted()
    {
        Debug.Log(selectedSpell.name + " casted");
        Events.RelayByValue<Spell>(SpellEvents.OnSpellSelected, PrepareCast);
        Events.ZipCall<bool>(SpellEvents.OnSpellCasted, true);
    }
}
