using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Challenge : ScriptableObject
{
    public event Action onCompleted;
    public event Action onFailed;
    
    public bool IsActive { get; private set; }

    public Sprite Icon => icon;
    [SerializeField] private Sprite icon;

    public string Title => title;
    [SerializeField] private string title;
    
    protected Player target;

    public abstract string GetDescription();
    
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