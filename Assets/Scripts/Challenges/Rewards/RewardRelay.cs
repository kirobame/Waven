﻿using System;
using Flux;
using Flux.Data;
using UnityEngine;
using System.Linq;

public class RewardRelay : ITurnbound
{
    public event Action<Motive> onIntendedTurnStop;
    public event Action onFree;

    public RewardRelay(short initiative)
    {
        Initiative = initiative;
        IsBusy = false;
    }

    public string Name => "Reward";
    public bool IsBusy { get; private set; }
    public short Initiative { get; private set; }
    
    public Match Match { get; set; }
    
    private Player target;
    
    public void IncreaseBusiness() { }
    public void DecreaseBusiness() { }

    public void SetTarget(Player player) => target = player;

    public void Activate()
    {
        if (!target.WasSuccessful || target.GetComponent<SpellDeck>().Spells.ToArray().Length >= 9)
        {
            Routines.Start(Routines.DoAfter(() => onIntendedTurnStop?.Invoke(new IntendedStopMotive()), 0.6f));
            return;
        }
        
        var handler = Repository.Get<RewardHandler>(References.Reward);
        handler.onHideStart += OnHideStart;
        handler.onDone += OnDone;
        
        handler.ShowFor(target);
    }
    public void Interrupt(Motive motive)
    {
        var handler = Repository.Get<RewardHandler>(References.Reward);
        handler.onHideStart -= OnHideStart;
        handler.onDone -= OnDone;
        
        if (IsBusy) return;
        handler.Hide();
    }

    void OnHideStart() => IsBusy = true;
    void OnDone()
    {
        onIntendedTurnStop?.Invoke(new IntendedStopMotive());
        
        IsBusy = false;
        onFree?.Invoke();
    }
}