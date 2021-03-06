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
        Events.RelayByValue<SpellBase>(GameEvent.OnSpellUsed, OnSpellUsed);
    }

    private void OnSpellUsed(SpellBase spell)
    {
        if (Spell == spell) gameObject.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Events.ZipCall<SpellBase>(InterfaceEvent.OnSpellSelected, Spell);
    }
    
    void OnDestroy()
    {
        Events.Unregister(GameEvent.OnTurnStart, OnTurnStart);
        Events.BreakValueRelay<SpellBase>(GameEvent.OnSpellUsed, OnSpellUsed);
    }

    private void OnTurnStart(EventArgs obj)
    {
        if (this.gameObject == null) return;
        if (!this.gameObject.activeSelf) this.gameObject.SetActive(true);
    }
}
