using System;
using Flux;
using UnityEngine;

[Serializable]
public abstract class MutableBoost<T> : Boost where T : IMutable
{
    public abstract Id Id { get; }
    
    [SerializeField] protected int value;
    [SerializeField] private bool temporary;
    [SerializeField] private int duration;
    [SerializeField] private bool self = true;

    public override CastArgs GetBoost()
    {
        CastArgs boost;
        
        if (temporary) boost = new TemporaryIntCastArgs(duration, self, value);
        else boost = new IntCastArgs(value);
        
        boost.SetId(Id);
        return boost;
    }

    protected override void HandleTarget(ITileable target, IAttributeHolder attributes)
    {
        var component = (Component)target;
        if (!component.TryGetComponent<T>(out var mutable)) return;
        
        base.HandleTarget(target, attributes);
        mutable.Dirty();
    }
}