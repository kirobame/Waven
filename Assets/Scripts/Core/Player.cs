using System;
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

    public bool IsBusy => business > 0;
    
    public string Name => name;
    public int Index => index;

    public Animator Animator => animator;
    
    public Match Match { get; set; }
    public short Initiative => initiative;

    [Space, SerializeField] private int index;
    [SerializeField] private short initiative;
    
    [Space, SerializeField] private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private Vector2Int orientation;

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
        
        SetOrientation(orientation);
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
        Events.Register(GameEvent.OnBaseAttack, OnBaseAttack);
    }
    
    public void Interrupt(Motive motive)
    {
        isActive = false;
        foreach (var activable in links) activable.Deactivate();
    }

    //------------------------------------------------------------------------------------------------------------------/

    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false, bool processDir = true)
    {
        IncreaseBusiness();
        
        if (speed <= 0 || !overrideSpeed) speed = this.speed;
        base.Move(path, speed, overrideSpeed, processDir);

        if (processDir) animator.SetBool("isMoving", true);
    }

    protected override void ProcessMoveDirection(Vector2 direction)
    {
        var orientation = direction.ComputeOrientation();
        SetOrientation(orientation);
    }
    public override void SetOrientation(Vector2Int direction)
    {
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }

    protected override void OnMoveCompleted()
    {
        DecreaseBusiness();
        animator.SetBool("isMoving", false);
    }

    //------------------------------------------------------------------------------------------------------------------/

    void OnBaseAttack(EventArgs obj)
    {
        if (isActive) animator.SetTrigger("isBaseAttack");
    }

    void OnSpellUsed(EventArgs obj)
    {
        if (!isActive) return;
        
        if (Buffer.consumeTriggerSpell) animator.SetTrigger("isCastingSpell");
        Buffer.consumeTriggerSpell = false;
    }
}
