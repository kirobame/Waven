using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using Flux.Data;
using Flux.Audio;
using Flux.Feedbacks;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : ButtonFeedback
{
    public SpellBase Spell => relay.Value;
    [SerializeField] protected SpellHolder relay;
    
    [Space, SerializeField] private Image image;
    [SerializeField] private Animator animator;
    
    [Space, SerializeField] private Sequencer use;

    private bool hasBeenUsed;
    private bool isActive;
    
    protected override void Awake()
    {
        base.Awake();

        relay.onSet += OnSpellSet;
        
        Events.RelayByVoid(InterfaceEvent.OnSpellInterruption, OnInterrupt);
        Events.RelayByValue<SpellBase,bool>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
        Events.RelayByValue<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
    }
    void OnDestroy()
    {
        Events.BreakVoidRelay(InterfaceEvent.OnSpellInterruption, OnInterrupt);
        Events.BreakValueRelay<SpellBase,bool>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
        Events.BreakValueRelay<SpellBase, bool>(GameEvent.OnSpellUsed, OnSpellUsed);
    }

    protected void OnSpellSet(SpellBase spell)
    {
        IsOperational = true;
        IsLocked = false;
        
        image.transform.localScale = Vector3.one;
        image.transform.rotation = Quaternion.identity;
        
        var color = image.color;
        color.a = 0.8f;
        image.color = color;

        animator.enabled = false;
        hasBeenUsed = false;
    }
    
    protected override void OnClick()
    {
        if (!Buffer.isGameTurn || Spellcaster.Active.RemainingUse <= 0) return;

        if (Spellcaster.Active != null && Spellcaster.Active.Current == Spell)
        {
            Events.Call(InputEvent.OnInterrupt, new WrapperArgs<bool>(true));
            isActive = false;
        }
        else
        {
            AudioHandler.Play(Repository.Get<AudioClipPackage>(AudioReferences.MouseClickOnClickableUI));
            Events.ZipCall<SpellBase, bool>(InterfaceEvent.OnSpellSelected, Spell, false);
            
            isActive = true;
        }
    }

    protected override void OnDiscardedClick() => AudioHandler.Play(Repository.Get<AudioClipPackage>(AudioReferences.MouseClickOnLockedUI));

    void OnInterrupt()
    {
        if (isActive)
        {
            TurnOff();
            isActive = false;
        }
    }
    void OnSpellSelected(SpellBase spell, bool isStatic)
    {
        if (isStatic) return;

        if (isActive && Spell != spell)
        {
            TurnOff();
            isActive = false;
        }
    }

    void OnSpellCastImpossible(SpellBase spell)
    {
        if (isActive && Spell != spell)
        {
            TurnOff();
            isActive = false;
        }
    }
    void OnSpellUsed(SpellBase spell, bool isStatic)
    {
        if (isStatic) return;

        if (isActive && Spell == spell)
        {
            hasBeenUsed = true;
            use.Play(EventArgs.Empty);
        }

        if (Spellcaster.Active.RemainingUse <= 0 && !hasBeenUsed)
        {
            IsOperational = false;
            animator.enabled = false;
            
            image.transform.localScale = Vector3.one;
            image.transform.rotation = Quaternion.identity;
            
            var color = image.color;
            color.a = 0.5f;
            image.color = color;
        }
        isActive = false;
    }
}