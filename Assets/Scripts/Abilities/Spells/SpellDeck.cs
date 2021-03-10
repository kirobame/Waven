﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Flux.Data;
using Flux.Event;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpellDeck : MonoBehaviour, ILink
{
    public event Action<ILink> onDestroyed;
    
    public ITurnbound Owner { get; set; }
    public IEnumerable<SpellBase> Spells => deck.Concat(hand);
    
    [Space, SerializeField] private int maxSpellInHand = 4;
    [SerializeField] private Spellcaster caster;
    
    private List<SpellBase> deck;
    private List<SpellBase> hand = new List<SpellBase>();

    private Player player;
    private bool hasBeenBootedUp;

    //------------------------------------------------------------------------------------------------------------------/

    void Awake() => player = GetComponent<Player>();
    void Start()
    {
        if (!Repository.TryGet<Spells>(References.Spells, out var spells)) return;
        deck = spells.GetDeck(3).ToList();
        
        hasBeenBootedUp = true;
    }

    public void Activate()
    {
        if (hasBeenBootedUp) Draw(2);
        else
        {
            if (!Repository.TryGet<Spells>(References.Spells, out var spells)) return;
            deck = spells.GetDeck(3).ToList();
            
            Draw(3);
            hasBeenBootedUp = true;
        }
        
        Events.RelayByVoid(InterfaceEvent.OnInfoRefresh, RefreshHotbar);
        Events.RelayByValue<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
    }
    public void Deactivate()
    {
        Events.BreakVoidRelay(InterfaceEvent.OnInfoRefresh, RefreshHotbar);
        Events.BreakValueRelay<SpellBase, bool>(GameEvent.OnSpellUsed, OnSpellUsed);
    }

    void OnDestroy() => onDestroyed?.Invoke(this);
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public void RefreshHotbar()
    {
        if (!Repository.TryGet<Hotbar>(References.Hotbar, out var hotbar)) return;
        
        hotbar.DisplaySpells(hand);
        hotbar.DisplayPA(caster.RemainingUse);
    }

    public void Draw(int nb)
    {
        if (nb > deck.Count) nb = deck.Count; //Not enough cards in deck

        for (int i = 0; i < nb; i++)
        {
            if (hand.Count >= maxSpellInHand) //Hand full
            {
                RefreshHotbar();
                return;
            }

            var spell = deck[0];
            deck.RemoveAt(0);
            hand.Add(spell);
        }
        RefreshHotbar();
    }

    public void Add(SpellBase spell)
    {
        if (hand.Count < 4)
        {
            hand.Add(spell);
            RefreshHotbar();
        }
        else deck.Add(spell);
    }
    public void Discard(SpellBase spell)
    {
        hand.Remove(spell);
        deck.Add(spell);
        RefreshHotbar();
    }

    //------------------------------------------------------------------------------------------------------------------/

    void OnSpellUsed(SpellBase spell, bool isStatic)
    {
        if (!isStatic) Discard(spell);
    }
}
