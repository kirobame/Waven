﻿using System;
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
    
    [SerializeField, Range(0, 5)] int movementPoints;

    public ITurnbound Owner { get; set; }
    public int PM
    {
        get => trueMovementPoints;
        set => trueMovementPoints = value;
    }
    private int trueMovementPoints;

    private bool hasCaster;
    private ITempCaster caster;
    
    //------------------------------------------------------------------------------------------------------------------/
    
    void Awake()
    {
        PM = movementPoints;
        hasCaster = TryGetComponent<ITempCaster>(out caster);
    }
    void OnDestroy() => onDestroyed?.Invoke(this);
    
    //------------------------------------------------------------------------------------------------------------------/

    public void Activate()
    {
        trueMovementPoints = movementPoints;
        Dirty();
    }
    public void Deactivate() { }
    
    //------------------------------------------------------------------------------------------------------------------/

    public void Dirty()
    {
        if (hasCaster && caster.Args.TryGetValue(new Id('M', 'V', 'T'), out var args) && args is IntCastArgs intArgs) trueMovementPoints += intArgs.Value;
    }
}