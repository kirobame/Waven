using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : ExtendedTileable, ITurnbound
{
    public static Player Active { get; private set; }
    
    public event Action onFree;
    public event Action<Motive> onIntendedTurnStop;

    public IReadOnlyList<ILink> Links => links;
    public bool IsBusy => business > 0;
    public bool WasSuccessful { get; set; }
    
    public string Name => name;
    public int Index => index;
    
    public Match Match { get; set; }
    public short Initiative => initiative;

    [Space, SerializeField] private int index;
    [SerializeField] private short initiative;

    private InputAction spacebarAction;
    
    private ushort business;
    
    private List<ILink> links;
    private bool isActive;

    //------------------------------------------------------------------------------------------------------------------/

    protected override void Start()
    {
        Repository.SetAt(References.Players, index, this);
        
        links = new List<ILink>();
        links.AddRange(GetComponentsInChildren<ILink>());
        foreach (var link in links) SetupLink(link);
        
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        spacebarAction = inputs["Core/Spacebar"];
        spacebarAction.performed += OnSpacebarPressed;
        
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        Match.Remove(this);
        foreach (var link in links) link.Owner = null;
        links.Clear();
        
        spacebarAction.performed -= OnSpacebarPressed;
    }

    void OnSpacebarPressed(InputAction.CallbackContext context) => OnEndTurn();

    public void OnEndTurn()
    {
        if (!isActive) return;
        Routines.Start(Routines.DoAfter(() =>
        {
            onIntendedTurnStop?.Invoke(new IntendedStopMotive());

        }, new YieldFrame()));
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    public void IncreaseBusiness() => business++;
    public void DecreaseBusiness()
    {
        business--;
        if (business <= 0 && isActive) onFree?.Invoke();
    }

    //------------------------------------------------------------------------------------------------------------------/

    private void SetupLink(ILink link)
    {
        link.Owner = this;
        link.onDestroyed += OnLinkDestruction;
        
        if (isActive) link.Activate();
    }
    
    public void AddDependency(GameObject source)
    {
        var links = source.GetComponentsInChildren<ILink>();
        foreach (var link in links) SetupLink(link);
        
        this.links.AddRange(links);
    }
    public void RemoveDependency(GameObject source)
    {
        var links = source.GetComponentsInChildren<ILink>();
        foreach (var link in links)
        {
            link.onDestroyed -= OnLinkDestruction;
            this.links.Remove(link);
        }
    }

    void OnLinkDestruction(ILink link)
    {
        link.onDestroyed -= OnLinkDestruction;
        links.Remove(link);
    }
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public void Activate()
    {
        Buffer.isGameTurn = true;
        Inputs.isLocked = false;
        
        Active = this;
        isActive = true;
        
        foreach (var activable in links) activable.Activate();
        Events.RelayByValue<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);

        if (!Repository.TryGet<Hotbar>(References.Hotbar, out var hotbar)) return;
        if (!gameObject.TryGet<Moveable>(out var moveable)) return;
        moveable.Dirty();
        hotbar.DisplayPM(moveable.PM);
    }
    
    public void Interrupt(Motive motive)
    {
        Buffer.isGameTurn = false;
        
        isActive = false;
        foreach (var activable in links) activable.Deactivate();
        
        Events.BreakValueRelay<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
    }

    //------------------------------------------------------------------------------------------------------------------/

    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false, bool processDir = true)
    {
        IncreaseBusiness();
        base.Move(path, speed, overrideSpeed, processDir);
    }
    protected override void OnMoveCompleted()
    {
        DecreaseBusiness();
        base.OnMoveCompleted();
    }

    //------------------------------------------------------------------------------------------------------------------/

    void OnSpellUsed(SpellBase spell, bool isStatic)
    {
        if (!isActive || isStatic) return;

        if (Buffer.consumeTriggerSpell) Animator.SetTrigger("isCastingSpell");
        Buffer.consumeTriggerSpell = false;
    }
}
