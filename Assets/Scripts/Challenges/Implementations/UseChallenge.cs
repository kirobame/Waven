using System;
using UnityEngine;

public abstract class UseChallenge : Challenge
{
    [SerializeField] protected int goal;
    
    private bool hasFailed;
    private int counter;
    
    protected override void OnTurnedOn()
    {
        hasFailed = false;
        counter = 0;
    }
    protected override void OnTurnedOff()
    {
        if (hasFailed) return;

        if (counter == goal) Complete();
        else Fail();
    }

    protected void OnAction(int value)
    {
        counter += value;
        if (counter <= goal) return;
        
        hasFailed = true;
        Fail();
    }
}