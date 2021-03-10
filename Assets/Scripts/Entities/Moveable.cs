using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Flux;
using Flux.Data;
using Flux.Event;
using Sirenix.Utilities;

public class Moveable : Navigator, ILink
{
    public event Action<ILink> onDestroyed;
    
    [SerializeField, Range(0, 5)] public int movementPoints;

    public ITurnbound Owner { get; set; }

    public int Max 
    {
        get
        {
            var output = movementPoints;
            if (hasCaster && caster.Args.TryAggregate(new Id('M', 'V', 'T'), out var result)) output += result;

            return output;
        }
    }
    public int PM
    {
        get => trueMovementPoints;
        set
        {
            var localDifference = value - trueMovementPoints;
            difference += localDifference;

            Events.ZipCall(ChallengeEvent.OnMove, -localDifference);
            
            trueMovementPoints = value;
            Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
        }
    }

    private int trueMovementPoints;
    private int difference;

    private bool hasCaster;
    private IAttributeHolder caster;
    
    //------------------------------------------------------------------------------------------------------------------/

    protected override void Start()
    {
        base.Start();
        
        trueMovementPoints = movementPoints; 
        hasCaster = TryGetComponent<IAttributeHolder>(out caster);
    }
    void OnDestroy() => onDestroyed?.Invoke(this);
    
    //------------------------------------------------------------------------------------------------------------------/

    public void Activate()
    {
        difference = 0;
        trueMovementPoints = movementPoints;
        
        Dirty();
    }
    public void Deactivate()
    {
        difference = 0;
        trueMovementPoints = movementPoints;
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    public void Dirty()
    {
        trueMovementPoints = movementPoints + difference;
        if (hasCaster && caster.Args.TryAggregate(new Id('M', 'V', 'T'), out var result)) trueMovementPoints += result;
    }
}