using System;
using Flux;
using Flux.Event;
using UnityEngine;

public class TimedTurn : Turn
{
    public TimedTurn(float duration, Enum startAddress, Enum timerAddress, Enum endAddress)
    {
        this.duration = duration;
        
        this.startAddress = startAddress;
        this.timerAddress = timerAddress;
        this.endAddress = endAddress;
    }

    private float duration;
    private Enum startAddress;
    private Enum timerAddress;
    private Enum endAddress;
    
    private Coroutine routine;

    protected override void OnStart()
    {
        Events.ZipCall(startAddress, (Turn)this);
        Events.ZipCall(timerAddress, true);

        routine = Routines.Start(Routines.RepeatFor(duration, ratio =>
        {
            Events.ZipCall(timerAddress, ratio);
            
        }).Chain(Routines.Do(() =>
        {
            var stopMotive = new TimeoutMotive();
            Stop(stopMotive);

            routine = null;
        })));
    }
    protected override void OnEnd()
    {
        Events.ZipCall(timerAddress, false);
        Events.ZipCall(endAddress, (Turn)this);
    }

    public override void Interrupt(Motive motive)
    {
        if (routine == null) return;
        
        Routines.Stop(routine);
        routine = null;
    }
}