using Flux;
using Flux.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux.Data;
using Flux.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfos : MonoBehaviour
{
    [SerializeField] private Player player;
    
    [Space, SerializeField] private Sequencer on;
    [SerializeField] private Sequencer off;
    [SerializeField] private Sequencer dead;

    [Space, SerializeField] private Image image;
    [SerializeField] private TMP_Text titleMesh;
    
    [Space, SerializeField] private Slider health;
    [SerializeField] private TMP_Text healthMesh;

    [Space, SerializeField] private TMP_Text deckSizeMesh;
    [SerializeField] private List<PlayerBuff> buffs;

    private bool state;
    
    private IDamageable damageable;
    private int cachedMaxHealth;

    private IAttributeHolder attributes;
    private Dictionary<StatType, PlayerBuff> keyedBuffs;

    private SpellDeck deck;

    void Awake()
    {
        state = false;
        
        Events.RelayByValue<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        Events.RelayByValue<Player>(GameEvent.OnPlayerDeath, OnPlayerDeath);
        Events.RelayByVoid(InterfaceEvent.OnInfoRefresh, Refresh);
        
        keyedBuffs = new Dictionary<StatType, PlayerBuff>();
        foreach (var buff in buffs)
        {
            if (keyedBuffs.ContainsKey(buff.Type)) continue;
            keyedBuffs.Add(buff.Type, buff);
        }
        
        damageable = player.GetComponent<IDamageable>();
        cachedMaxHealth = damageable.Get("Health").maxValue;

        attributes = player.GetComponent<IAttributeHolder>();

        deck = player.GetComponent<SpellDeck>();
    }

    void OnDestroy()
    {
        Events.BreakValueRelay<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        Events.BreakValueRelay<Player>(GameEvent.OnPlayerDeath, OnPlayerDeath);
        Events.BreakVoidRelay(InterfaceEvent.OnInfoRefresh, Refresh);
    }

    void OnTurnStart(Turn turn)
    {
        if (!(turn.Target is Player player)) return;
        
        if (this.player != player)
        {
            if (state)
            {
                off.Play(EventArgs.Empty, on);
                state = false;
            }
            
            return;
        }
        else
        {
            if (!state)
            {
                on.Play(EventArgs.Empty, off);
                state = true;
            }
        }
    }

    void OnPlayerDeath(Player player)
    {
        if (this.player != player) return;

        health.value = 0;
        healthMesh.text = $"0/{cachedMaxHealth}";
        
        dead.Play(EventArgs.Empty, on, off);
    }

    void Refresh()
    {
        var life = damageable.Get("Health");
        health.value = (float)life.actualValue / cachedMaxHealth;
        healthMesh.text = $"{life.actualValue}/{cachedMaxHealth}";
        
        HandleBuff(new Id('M','V','T'), StatType.Movement);
        HandleBuff(new Id('D','M','G'), StatType.Damage);
        HandleBuff(new Id('P','S','H'), StatType.Force);

        var spellsCount = deck.Spells.Count();
        deckSizeMesh.text =  spellsCount.ToString();
    }

    private void HandleBuff(Id id, StatType type)
    {
        if (!attributes.Args.TryGetValue(id, out var args))
        {
            HideBuff(type);
            return;
        }

        var value = 0;
        foreach (var arg in args)
        {
            if (!(arg is IWrapper<int> wrapper) || arg is FoundingWrapperCastArgs<int>) continue;
            value += wrapper.Value;
        }

        if (value != 0)
        {
            var buff = keyedBuffs[type];
            
            if (!buff.IsActive) buff.Show(value);
            else buff.Refresh(value);
        }
        else HideBuff(type);
    }

    private void HideBuff(StatType type)
    {
        var buff = keyedBuffs[type];
        if (buff.IsActive) buff.Hide();
    }
}
