using Flux;
using UnityEngine;
using System;

[Serializable]
public class TemporaryWrapperCastArgs<T> : TemporaryCastArgs, IWrapper<T>
{
    public TemporaryWrapperCastArgs(int duration, T value) : base(duration) => this.value = value;   
    
    public T Value => value;
    [SerializeField] private T value;
    
    public override CastArgs Copy() => new WrapperCastArgs<T>(value);
}