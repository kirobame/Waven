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
        uint casterMana = 10;                // -> caster.mana
        if (!CanBeCasted(casterMana))
        {
            Events.ZipCall<bool>(SpellEvents.OnSpellCasted, false);
            return;
        }

        caster = default;                   //get current caster
        selectedSpell = _selectedSpell;

        Events.BreakValueRelay<Spell>(SpellEvents.OnSpellSelected, PrepareCast);
        Events.RelayByValue<Vector2>(SpellEvents.OnPosSelected, Cast);
    }

    void Cast(Vector2 pos)
    {
        List<Vector2> castPos = new List<Vector2>();

        Events.BreakValueRelay<Vector2>(SpellEvents.OnPosSelected, Cast);

        //Get affected tiles -> CastPos


        foreach (var effect in selectedSpell.Effects())
        {
            effect(castPos);
        }

        //caster.mana -= spell.cost
        Casted();
    }

    void Casted()
    {
        Events.RelayByValue<Spell>(SpellEvents.OnSpellSelected, PrepareCast);
        Events.ZipCall<bool>(SpellEvents.OnSpellCasted, true);
    }
}
