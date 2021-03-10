using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpellButton : GameButton
{
    public SpellBase Spell => relay.Value;
    [SerializeField] protected SpellHolder relay;

    protected override void OnClick(PointerEventData eventData)
    {
        if (SpellDeck.RemainingUse <= 0) return;

        if (Spellcaster.Active != null && Spellcaster.Active.Current == Spell) Events.Call(InputEvent.OnInterrupt, new WrapperArgs<bool>(true));
        else Events.ZipCall<SpellBase, bool>(InterfaceEvent.OnSpellSelected, Spell, false);
    }
}