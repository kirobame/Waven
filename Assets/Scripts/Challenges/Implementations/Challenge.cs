using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public abstract class Challenge
{
    public event Action onCompleted;
    public event Action onFailed;
    
    public bool IsActive { get; private set; }
    
    protected Player target;
    
    public ChallengeInfo GetInfo() => new ChallengeInfo();
    
    public void TurnOn(Player player)
    {
        if (IsActive) return;
        
        target = player;
        IsActive = true;
        OnTurnedOn();
    }
    protected abstract void OnTurnedOn();

    public void TurnOff()
    {
        if (!IsActive) return;
        
        IsActive = false;
        OnTurnedOff();
    }
    protected abstract void OnTurnedOff();

    protected void Complete() => onCompleted?.Invoke();
    protected void Fail() => onFailed?.Invoke();
}