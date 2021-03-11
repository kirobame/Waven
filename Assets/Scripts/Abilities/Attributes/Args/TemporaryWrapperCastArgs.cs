using Flux;
using UnityEngine;
using System;

[Serializable]
public class TemporaryWrapperCastArgs<T> : TemporaryCastArgs, IWrapper<T>
{
    public TemporaryWrapperCastArgs(int duration, bool self, T value) : base(duration, self) => this.value = value;   
    
    public T Value => value;
    [SerializeField] private T value;
    
    public override CastArgs Copy()
    {
        var args = new TemporaryWrapperCastArgs<T>(duration, self, value);
        args.SetId(id);

        return args;
    }
}