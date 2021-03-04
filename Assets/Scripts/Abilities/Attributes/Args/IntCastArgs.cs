using System;

[Serializable]
public class IntCastArgs : WrapperCastArgs<int>
{
    public IntCastArgs(int value) : base(value) { }
}