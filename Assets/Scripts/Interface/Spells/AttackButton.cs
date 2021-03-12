using System;
using Flux;
using Flux.Audio;
using Flux.Data;
using Flux.Event;
using Flux.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButton : ButtonFeedback
{
    [SerializeField] private SpellBase spell;
    
    [Space, SerializeField]  private Image button;
    [SerializeField] private Animator animator;
    
    [SerializeField] private Sequencer use;

    private Pathfinder Pathfinder => Player.Active.GetComponent<Pathfinder>();
    
    private bool inUse;

    protected override void Awake()
    {
        base.Awake();
        
        Events.RelayByVoid(InterfaceEvent.OnSpellInterruption, OnInterrupt);
        
        Events.RelayByVoid(GameEvent.OnTurnStart, OnTurnStart);
        Events.RelayByVoid(GameEvent.OnTurnEnd, OnTurnEnd);
        
        Events.RelayByValue<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
        Events.RelayByVoid(ChallengeEvent.OnAttack, OnAttack);
    }
    void OnDestroy()
    {
        Events.BreakVoidRelay(InterfaceEvent.OnSpellInterruption, OnInterrupt);
        
        Events.BreakVoidRelay(GameEvent.OnTurnStart, OnTurnStart);
        Events.RelayByVoid(GameEvent.OnTurnEnd, OnTurnEnd);
        
        Events.BreakValueRelay<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
        Events.BreakVoidRelay(ChallengeEvent.OnAttack, OnAttack);
    }
    
    void OnTurnStart()
    {
        Routines.Start(Routines.DoAfter(() => Pathfinder.onDirtied += OnDirtied, new YieldFrame()));
        
        Activate();
        inUse = false;
    }
    void OnTurnEnd() => Pathfinder.onDirtied -= OnDirtied;
    
    protected override void OnClick()
    {
        if (!inUse)
        {
            inUse = true;
            Events.ZipCall(InterfaceEvent.OnSpellSelected, spell, true);
        }
        else
        {
            Events.Call(InputEvent.OnInterrupt, new WrapperArgs<bool>(true));
            inUse = false;
        }
    }
    protected override void OnDiscardedClick() => AudioHandler.Play(Repository.Get<AudioClipPackage>(AudioReferences.MouseClickOnLockedUI));

    void OnDirtied(Pathfinder source)
    {
        if (Pathfinder.AttackCounter <= 0) Deactivate();
        else Activate();
    }
    void OnAttack()
    {
        if (Pathfinder.AttackCounter == 0) Deactivate();
    }
    
    void OnInterrupt()
    {
        if (inUse)
        {
            TurnOff();
            inUse = false;
        }
    }
    
    void OnSpellUsed(SpellBase spell, bool isStatic)
    {
        if (inUse && spell == this.spell)
        {
            var pathfinder = Pathfinder;
            
            pathfinder.AttackCounter--;
            Events.EmptyCall(ChallengeEvent.OnAttack);

            if (pathfinder.AttackCounter == 0) Deactivate();
            else TurnOff();
        }
        
        inUse = false;
    }
    
    private void Activate()
    {
        IsOperational = true;
        IsLocked = false;
        
        button.transform.localScale = Vector3.one;
        button.transform.localRotation = Quaternion.identity;
        
        var color = button.color;
        color.a = 0.8f;
        button.color = color;
        
        animator.enabled = false;
    }

    private void Deactivate()
    {
        IsOperational = false;
        use.Play(EventArgs.Empty);
    }

    /*[SerializeField] private Image button;
    [SerializeField] private SpellBase spell;
    
    private Pathfinder Pathfinder => Player.Active.GetComponent<Pathfinder>();
    
    private bool inUse;
    
    void Awake()
    {
        Events.RelayByValue<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
        Events.RelayByVoid(GameEvent.OnTurnStart, OnTurnStart);
        Events.RelayByVoid(GameEvent.OnTurnEnd, OnTurnEnd);
        Events.RelayByVoid(ChallengeEvent.OnAttack, OnAttack);
    }
    void OnDestroy()
    {
        Events.BreakValueRelay<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
        Events.BreakVoidRelay(GameEvent.OnTurnStart, OnTurnStart);
        Events.RelayByVoid(GameEvent.OnTurnEnd, OnTurnEnd);
        Events.BreakVoidRelay(ChallengeEvent.OnAttack, OnAttack);
    }

    protected override void OnClick(PointerEventData eventData)
    {
        if (!inUse)
        {
            inUse = true;
            Events.ZipCall(InterfaceEvent.OnSpellSelected, spell, true);
        }
        else
        {
            Events.Call(InputEvent.OnInterrupt, new WrapperArgs<bool>(true));
            inUse = false;
        }
    }

    void OnTurnStart()
    {
        Routines.Start(Routines.DoAfter(() => Pathfinder.onDirtied += OnDirtied, new YieldFrame()));
        
        inUse = false;
        Activate();
    }
    void OnTurnEnd() => Pathfinder.onDirtied -= OnDirtied;
    

    void OnDirtied(Pathfinder source)
    {
        if (Pathfinder.AttackCounter <= 0) Deactivate();
        else Activate();
    }
    void OnAttack()
    {
        if (Pathfinder.AttackCounter == 0) Deactivate();
    }

    void OnSpellUsed(SpellBase spell, bool isStatic)
    {
        if (inUse && spell == this.spell)
        {
            var pathfinder = Pathfinder;
            
            pathfinder.AttackCounter--;
            Events.EmptyCall(ChallengeEvent.OnAttack);
            
            if (pathfinder.AttackCounter == 0) Deactivate();
        }
        inUse = false;
    }

    private void Activate()
    {
        var color = button.color;
        color.a = 1.0f;
        button.color = color;

        button.raycastTarget = true;
    }
    private void Deactivate()
    {
        var color = button.color;
        color.a = 0.6f;
        button.color = color;

        button.raycastTarget = false;
    }*/
}