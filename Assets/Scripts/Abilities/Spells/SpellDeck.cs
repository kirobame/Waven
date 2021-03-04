using System.Collections;
using System.Collections.Generic;
using System;
using Flux.Data;
using Flux.Event;
using UnityEngine;

public class SpellDeck : MonoBehaviour, ILink
{
    public static int RemainingUse { get; private set; }
    public event Action<ILink> onDestroyed;
    
    public ITurnbound Owner { get; set; }

    [SerializeField] private int maxUse;
    [SerializeField] private int maxSpellInHand = 4;
    [SerializeField] private List<SpellBase> deck = new List<SpellBase>();
    
    private List<SpellBase> hand = new List<SpellBase>();

    //------------------------------------------------------------------------------------------------------------------/

    private void Awake() => Shuffle();
    void OnDestroy() => onDestroyed?.Invoke(this);
    
    public void Activate()
    {
        RemainingUse = maxUse;
        
        Draw(2);
        Events.RelayByValue<SpellBase>(GameEvent.OnSpellUsed, OnSpellUsed);
    }
    public void Deactivate() => Events.BreakValueRelay<SpellBase>(GameEvent.OnSpellUsed, OnSpellUsed);
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public void RefreshHotbar() => Repository.Get<Hotbar>(References.Hotbar).DisplaySpells(hand);

    public void Draw(int nb)
    {
        if (nb > deck.Count)
            nb = deck.Count; //Not enough cards in deck

        for (int i = 0; i < nb; i++)
        {
            if (hand.Count >= maxSpellInHand)//Hand full
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

    public void Discard(SpellBase spell)
    {
        hand.Remove(spell);
        deck.Add(spell);
        RefreshHotbar();
    }

    private static System.Random rng = new System.Random();
    public void Shuffle()
    {
        var n = deck.Count;
        while (n > 1)
        {
            n--;
            
            var k = rng.Next(n + 1);
            var value = deck[k];
            
            deck[k] = deck[n];
            deck[n] = value;
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    void OnSpellUsed(SpellBase spell)
    {
        RemainingUse--;
        Discard(spell);
    }
}
