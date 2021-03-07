﻿using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Tileable, ITurnbound
{
    public static Player Active { get; private set; }
    
    public event Action onFree;
    public event Action<Motive> onIntendedTurnStop;

    public string Name => name;
    public int Index => index;

    public bool IsBusy => business > 0;
    
    public Match Match { get; set; }
    public short Initiative => initiative;

    [Space, SerializeField] private int index;
    
    [Space, SerializeField] private Animator animator;
    [SerializeField] private short initiative;
    [SerializeField] private float speed;

    private InputAction spacebarAction;
    
    private ushort business;
    
    private List<ILink> links;
    private bool isActive;

    //------------------------------------------------------------------------------------------------------------------/
    
    void Start()
    {
        Repository.SetAt(References.Players, index, this);
        
        links = new List<ILink>();
        links.AddRange(GetComponentsInChildren<ILink>());
        foreach (var link in links) SetupLink(link);
        
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        spacebarAction = inputs["Core/Spacebar"];
        spacebarAction.performed += OnSpacebarPressed;
        
        ProcessMoveDirection(Vector2Int.right);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Events.ZipCall(GameEvent.OnPlayerDeath, index);
        
        Match.Remove(this);
        foreach (var link in links) link.Owner = null;
        links.Clear();
        
        spacebarAction.performed -= OnSpacebarPressed;
    }

    void OnSpacebarPressed(InputAction.CallbackContext context)
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

    void OnLinkDestruction(ILink link)
    {
        link.onDestroyed -= OnLinkDestruction;
        links.Remove(link);
    }
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public void Activate()
    {
        Inputs.isLocked = false;
        
        Active = this;
        
        isActive = true;
        foreach (var activable in links) activable.Activate();
        Events.Register(GameEvent.OnSpellUsed, OnSpellUsed);
    }
    

    public void Interrupt(Motive motive)
    {
        isActive = false;
        foreach (var activable in links) activable.Deactivate();
    }

    //------------------------------------------------------------------------------------------------------------------/

    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false)
    {
        IncreaseBusiness();
        
        if (speed <= 0 || !overrideSpeed) speed = this.speed;
        base.Move(path, speed, overrideSpeed);

        animator.SetBool("isMoving", true);
    }

    protected override void ProcessMoveDirection(Vector2 direction)
    {
        var orientation = direction.ComputeOrientation();
        animator.SetFloat("X", orientation.x);
        animator.SetFloat("Y", orientation.y);
    }

    protected override void OnMoveCompleted()
    {
        DecreaseBusiness();
        animator.SetBool("isMoving", false);
    }

    //------------------------------------------------------------------------------------------------------------------/

    void OnSpellUsed(EventArgs obj)
    {
        if (isActive) animator.SetTrigger("isCastingSpell");
    }
}
