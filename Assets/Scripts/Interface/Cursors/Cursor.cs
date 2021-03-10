using System;
using System.Collections;
using System.Collections.Generic;
using Flux.Event;
using UnityEngine;

public class Cursor : MonoBehaviour, ILink
{
    public event Action<ILink> onDestroyed;
    
    public ITurnbound Owner { get; set; }
    public bool IsActive { get; private set; }
    
    [SerializeField] private new SpriteRenderer renderer;
    
    void OnDestroy()
    {
        Deactivate();
        onDestroyed?.Invoke(this);
    }
    
    public void Activate()
    {
        Show();
        Events.RelayByValue<bool>(InputEvent.OnInputLock, OnInputLock);
    }
    public void Deactivate()
    {
        Hide();
        Events.BreakValueRelay<bool>(InputEvent.OnInputLock, OnInputLock);
    }

    public void Show()
    {
        IsActive = true;
        renderer.enabled = true;
    }

    public void Hide()
    {
        IsActive = false;
        renderer.enabled = false;
    }

    void OnInputLock(bool state)
    {
        if (state) Hide();
        else Show();
    }
}
