﻿using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    public SpellBase Spell => relay.Value;
    [SerializeField] protected SpellHolder relay;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (SpellDeck.RemainingUse <= 0) return;
        Events.ZipCall<SpellBase>(InterfaceEvent.OnSpellSelected, Spell);
    }
}
