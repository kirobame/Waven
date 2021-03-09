using System;
using Flux;
using Flux.Data;
using UnityEngine;

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
        Debug.Log($"REWARD START");
        if (!target.WasSuccessful)
        {
            Routines.Start(Routines.DoAfter(() => onIntendedTurnStop?.Invoke(new IntendedStopMotive()), new YieldFrame()));
            return;
        }
        
        var handler = Repository.Get<RewardHandler>(References.Reward);
        handler.onHideStart += OnHideStart;
        handler.onDone += OnDone;
        
        handler.ShowFor(target);
    }
    public void Interrupt(Motive motive)
    {
        Debug.Log($"REWARD END");
        
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