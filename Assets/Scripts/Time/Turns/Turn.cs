using System;
using System.Collections;
using Flux.Event;
using UnityEngine;

public abstract class Turn : IComparable<Turn>
{
    public event Action<Motive> onEnd;
    
    public ITurnbound Target { get; private set; }

    private bool hasBeenInterrupted;
    private Motive cachedMotive;
    
    //---[Registration]-------------------------------------------------------------------------------------------------/

    public void AssignTarget(ITurnbound target)
    {
        Target = target;
        target.onIntendedTurnStop += OnTargetIntendedStop;
    }
    
    //---[Lifetime]-----------------------------------------------------------------------------------------------------/

    public void Start()
    {
        hasBeenInterrupted = false;
        
        Events.ZipCall(GameEvent.OnTurnStart, this);
        Target.Activate();
        
        OnStart();
    }
    protected abstract void OnStart();

    protected void Stop(Motive motive)
    {
        if (Target.IsBusy)
        {
            cachedMotive = motive;
            Target.onFree += DelayedStop;
        }
        else End(motive);
    }

    private void DelayedStop()
    {
        Target.onFree -= DelayedStop;
        End(cachedMotive);
    }
    
    private void End(Motive motive)
    {
        Target.Interrupt(motive);
        
        OnEnd();
        onEnd?.Invoke(motive);
    }
    protected virtual void OnEnd() { }

    public abstract void Interrupt(Motive motive);
    private void DelayedInterruption()
    {
        Target.onFree -= DelayedInterruption;
        
        Interrupt(cachedMotive);
        End(cachedMotive);
    }
    
    protected virtual void OnTargetIntendedStop(Motive motive)
    {
        if (hasBeenInterrupted) return;
        hasBeenInterrupted = true;
        
        if (Target.IsBusy)
        {
            cachedMotive = motive;
            Target.onFree += DelayedInterruption;
        }
        else
        {
            Interrupt(motive);
            End(motive);
        }
    }
    
    //---[Sorting]------------------------------------------------------------------------------------------------------/

    int IComparable<Turn>.CompareTo(Turn other) => Target.Initiative.CompareTo(other.Target.Initiative);
}
