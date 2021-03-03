using Flux;
using Flux.Event;
using UnityEngine;

public class TimedTurn : Turn
{
    public TimedTurn(float duration) => this.duration = duration;
    
    private float duration;
    private Coroutine routine;
    
    protected override void OnStart()
    {
        Events.ZipCall(GameEvent.OnTurnTimer, true);

        routine = Routines.Start(Routines.RepeatFor(duration, ratio =>
        {
            Events.ZipCall(GameEvent.OnTurnTimer, ratio);
            
        }).Chain(Routines.Do(() =>
        {
            var stopMotive = new TimeoutMotive();
            Stop(stopMotive);

            routine = null;
        })));
    }
    protected override void OnEnd() => Events.ZipCall(GameEvent.OnTurnTimer, false);

    public override void Interrupt(Motive motive)
    {
        Routines.Stop(routine);
        routine = null;
    }
}