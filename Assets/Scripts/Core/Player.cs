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

    public string Name => name;

    public bool IsBusy => business > 0;
    
    public Match Match { get; set; }
    public short Initiative => initiative;

    [Space, SerializeField] private short initiative;
    
    private InputAction spacebarAction;

    private ushort business;
    
    private List<ILink> links;
    private bool isActive;
    
    //------------------------------------------------------------------------------------------------------------------/
    
    void Start()
    {
        SetOrientation(Vector2Int.right);
        
        links = new List<ILink>();
        links.AddRange(GetComponentsInChildren<ILink>());
        foreach (var link in links) SetupLink(link);
        
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        spacebarAction = inputs["Core/Spacebar"];
        spacebarAction.performed += OnSpacebarPressed;
    }
    void OnDestroy() => spacebarAction.performed -= OnSpacebarPressed;

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
    }
    public void Interrupt(Motive motive)
    {
        isActive = false;
        foreach (var activable in links) activable.Deactivate();
    }

    //------------------------------------------------------------------------------------------------------------------/

    public override void Move(Vector2[] path)
    {
        IncreaseBusiness();
        base.Move(path);
    }

    protected override void OnMoveCompleted() => DecreaseBusiness();
}
