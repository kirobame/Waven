using System;

[Serializable]
public class TemporaryIntCastArgs : TemporaryWrapperCastArgs<int>
{
    public TemporaryIntCastArgs(int duration, int value) : base(duration, value) { }
}