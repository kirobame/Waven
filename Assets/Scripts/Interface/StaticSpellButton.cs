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
        if (actualSpell == spell)
            this.gameObject.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Events.ZipCall<SpellBase>(InterfaceEvent.OnSpellSelected, actualSpell);
    }

    private void OnTurnStart(EventArgs obj)
    {
        if (!this.gameObject.activeSelf)
            this.gameObject.SetActive(true);
    }
}
