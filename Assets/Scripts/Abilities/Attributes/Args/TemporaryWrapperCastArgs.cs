using Flux;
using UnityEngine;
using System;

[Serializable]
public class TemporaryWrapperCastArgs<T> : TemporaryCastArgs, IWrapper<T>
{
    public TemporaryWrapperCastArgs(T value) => this.value = value;   
    
    public T Value => value;
    [SerializeField] private T value;
    
    public override CastArgs Copy() => new WrapperCastArgs<T>(value);
}