using Flux;
using UnityEngine;
using System;

[Serializable]
public class WrapperCastArgs<T> : CastArgs, IWrapper<T>
{
    public WrapperCastArgs(T value) => this.value = value;   
    
    public T Value => value;
    [SerializeField] private T value;
    
    public override CastArgs Copy()
    {
        var args = new WrapperCastArgs<T>(value);
        args.SetId(id);

        return args;
    }
}