﻿using System;
using UnityEngine;

[Serializable]
public abstract class ToggleChallenge : Challenge
{
    [SerializeField] private bool execute;
    
    private bool wasDone;

    protected override void OnTurnedOn()
    {
        wasDone = false;
    }
    protected override void OnTurnedOff()
    {
        if (wasDone) return;
        
        if (execute) Fail();
        else Complete();
    }

    protected virtual void OnAction(EventArgs args)
    {
        wasDone = true;
        
        if (execute) Complete();
        else Fail();
    }
}