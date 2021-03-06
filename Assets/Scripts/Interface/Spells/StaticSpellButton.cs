using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class StaticSpellButton : SpellButton, IPointerClickHandler
{
    private void Start()
    {
        Events.Register(GameEvent.OnTurnStart, OnTurnStart);
        Events.RelayByValue<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
    }

    private void OnSpellUsed(SpellBase spell, bool isStatic)
    {
        if (Spell == spell) gameObject.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Events.ZipCall<SpellBase,bool>(InterfaceEvent.OnSpellSelected, Spell, true);
    }
    
    void OnDestroy()
    {
        Events.Unregister(GameEvent.OnTurnStart, OnTurnStart);
        Events.BreakValueRelay<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
    }

    private void OnTurnStart(EventArgs obj)
    {
        if (this.gameObject == null) return;
        if (!this.gameObject.activeSelf) this.gameObject.SetActive(true);
    }
}
