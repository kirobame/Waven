using System;
using System.Collections;
using System.Collections.Generic;
using Flux.Event;
using Flux.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ButtonFeedback : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsOperational { get; set; }
    public bool IsLocked { get; set; }

    [SerializeField] private Timetable table;
    [SerializeField] private Sequencer on;
    [SerializeField] private Sequencer off;
    
    private SendbackArgs onArgs;
    private SendbackArgs offArgs;

    private bool hasHover;

    protected virtual void Awake()
    {
        IsOperational = true;
        IsLocked = false;
        
        hasHover = false;

        onArgs = new SendbackArgs();
        onArgs.onDone += OnTurnedOn;
        
        offArgs = new SendbackArgs();
        offArgs.onDone += OnTurnedOff;
    }

    void Update()
    {
        if (IsLocked ||!IsOperational) return;

        if (hasHover) table.MoveBy(Time.deltaTime);
        else table.MoveBy(-Time.deltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hasHover = true;
        OnHoverStart();
    }
    protected virtual void OnHoverStart() { }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        hasHover = false;
        OnHoverEnd();
    }
    protected virtual void OnHoverEnd() { }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsOperational)
        {
            OnDiscardedClick();
            return;
        }

        if (!IsLocked) TurnOn();
        else TurnOff();

        OnClick();
        IsOperational = false;
    }
    protected virtual void OnClick() { }

    protected void TurnOn()
    {
        if (IsLocked) return;
        
        OnSwitch(true);
        
        IsLocked = true;
        on.Play(onArgs);
    }
    protected void TurnOff()
    {
        if (!IsLocked) return;
        
        OnSwitch(false);
        
        IsLocked = false;
        off.Play(offArgs);
    }
    
    protected virtual void OnDiscardedClick() { }
    protected virtual void OnSwitch(bool state) { }

    protected virtual void OnTurnedOn(EventArgs args) => IsOperational = true;
    protected virtual void OnTurnedOff(EventArgs args) => IsOperational = true;
}