using System;

[Serializable]
public class TemporaryIntCastArgs : TemporaryWrapperCastArgs<int>
{
    public TemporaryIntCastArgs(int value) : base(value) { }
}