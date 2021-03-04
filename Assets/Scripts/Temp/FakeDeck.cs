using System.Collections;
using System.Collections.Generic;
using Flux.Data;
using UnityEngine;

public class FakeDeck : MonoBehaviour
{
    public List<Spell> deck = new List<Spell>();

    void Start()
    {
        var hotbar = Repository.Get<Hotbar>(References.Hotbar);
        hotbar.DisplaySpells(deck);
    }
}
