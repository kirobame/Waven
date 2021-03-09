using System;

[Serializable]
public class FoundingIntCastArgs : FoundingWrapperCastArgs<int>
{
    public FoundingIntCastArgs(int value) : base(value) { }
}