using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltPlayer : ITurnbound
{
    public AltPlayer(string name, short initiative)
    {
        var inputs = Repository.Get<InputActionAsset>(AltReferences.Inputs);
        inputs["Temp/OnSpace"].started += ctxt =>
        {
            if (routine == null) return;

            Routines.Start(Routines.DoAfter(() =>
            {
                Debug.Log($"{name} has finished his turn.");

                Routines.Stop(routine);
                routine = null;

                onIntendedTurnStop?.Invoke(new IntendedStopMotive());

            }, new YieldFrame()));
        };
        
        this.name = name;
        this.initiative = initiative;
    }

    public event Action<Motive> onIntendedTurnStop;
    
    public Match Match { get; set; }
    public short Initiative => initiative;
    
    private string name;
    private short initiative;

    private Coroutine routine;
    
    public void Activate()
    {
        Debug.Log($"{name} is starting his turn.");
        routine = Routines.Start(Routine());
    }

    private IEnumerator Routine()
    {
        while (true) yield return new WaitForEndOfFrame();
    }

    public void Interrupt(Motive motive)
    {
        Debug.Log($"{name} could not finish his turn in time !");
        
        Routines.Stop(routine);
        routine = null;
    }
}