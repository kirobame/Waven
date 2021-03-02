using System;
using System.Collections;
using UnityEngine;

public abstract class Turn : IComparable<Turn>
{
    public event Action<Motive> onEnd;
    
    public ITurnbound Target { get; private set; }
    
    //---[Registration]-------------------------------------------------------------------------------------------------/

    public void AssignTarget(ITurnbound target)
    {
        Target = target;
        target.onIntendedTurnStop += OnTargetIntendedStop;
    }
    
    //---[Lifetime]-----------------------------------------------------------------------------------------------------/

    public void Start()
    {
        Target.Activate();
        OnStart();
    }
    protected abstract void OnStart();
    
    protected void Stop(Motive motive)
    {
        Target.Interrupt(motive);
        onEnd?.Invoke(motive);
    }

    public abstract void Interrupt(Motive motive);

    //---[Callbacks]----------------------------------------------------------------------------------------------------/
    
    void OnTargetIntendedStop(Motive motive)
    {
        Interrupt(motive);
        onEnd?.Invoke(motive);
    }
    
    //---[Sorting]------------------------------------------------------------------------------------------------------/

    int IComparable<Turn>.CompareTo(Turn other) => Target.Initiative.CompareTo(other.Target.Initiative);
}
