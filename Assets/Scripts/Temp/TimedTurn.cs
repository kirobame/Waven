using Flux;
using UnityEngine;

public class TimedTurn : Turn
{
    public TimedTurn(float duration) => this.duration = duration;
    
    private float duration;
    private Coroutine routine;
    
    protected override void OnStart()
    {
        routine = Routines.Start(Routines.DoAfter(() =>
        {
            var stopMotive = new TimeoutMotive();
            Stop(stopMotive);
            
        }, duration));
    }
    
    public override void Interrupt(Motive motive)
    {
        Routines.Stop(routine);
        routine = null;
    }
}