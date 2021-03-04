using System.Collections;
using System.Collections.Generic;
using System;
using Flux.Data;
using UnityEngine;

public class SpellDeck : MonoBehaviour
{
    [SerializeField] List<Spell> deck = new List<Spell>();
    List<Spell> hand = new List<Spell>();
    int maxSpellInHand = 4;

    private void Awake() => Shuffle();
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

            Spell spell = deck[0];
            deck.RemoveAt(0);
            hand.Add(spell);
        }
        RefreshHotbar();
    }

    public void Discard(Spell spell)
    {
        hand.Remove(spell);
        deck.Add(spell);
        RefreshHotbar();
    }

    private static System.Random rng = new System.Random();
    public void Shuffle()
    {
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Spell value = deck[k];
            deck[k] = deck[n];
            deck[n] = value;
        }
    }
}
