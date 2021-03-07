using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Flux.Data;
using Flux.Event;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpellDeck : MonoBehaviour, ILink
{
    public static int RemainingUse { get; private set; }
    
    public event Action<ILink> onDestroyed;
    
    public ITurnbound Owner { get; set; }
    public IEnumerable<SpellBase> Spells => deck.Concat(hand);

    [ShowInInspector] public SpellBase[] GNEGNE => Spells.ToArray();
    
    [Space, SerializeField] private int maxUse;
    [SerializeField] private int maxSpellInHand = 4;
    
    private List<SpellBase> deck;
    private List<SpellBase> hand = new List<SpellBase>();

    private Player player;
    private bool hasBeenBootedUp;

    //------------------------------------------------------------------------------------------------------------------/

    void Awake()
    {
        player = GetComponent<Player>();
        hasBeenBootedUp = false;
    }

    public void Activate()
    {
        RemainingUse = maxUse;
        
        if (hasBeenBootedUp) Draw(2);
        else
        {
            var data = Repository.GetAt<PlayerData>(References.Data, player.Index);
            if (!data.IsValid || !data.Deck.Any()) deck = Repository.Get<Spells>(References.Spells).GetDeck(6).ToList();
            else
            {
                deck = data.Deck.ToList();
                deck.Shuffle();
            }
            
            Draw(3);
            hasBeenBootedUp = true;
        }
        
        Events.RelayByValue<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
    }
    public void Deactivate() => Events.BreakValueRelay<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
    
    void OnDestroy() => onDestroyed?.Invoke(this);
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public void RefreshHotbar()
    {
        if (!Repository.TryGet<Hotbar>(References.Hotbar, out var hotbar)) return;
        hotbar.DisplaySpells(hand);
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

    public void Discard(SpellBase spell)
    {
        hand.Remove(spell);
        deck.Add(spell);
        RefreshHotbar();
    }

    //------------------------------------------------------------------------------------------------------------------/

    void OnSpellUsed(SpellBase spell, bool isStatic)
    {
        if (!isStatic)
        {
            RemainingUse--;
            Discard(spell);
        }
    }
}
