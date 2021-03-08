using System;

[Serializable]
public class FoundingWrapperCastArgs<T> : WrapperCastArgs<T> 
{
    public FoundingWrapperCastArgs(T value) : base(value) { }
}