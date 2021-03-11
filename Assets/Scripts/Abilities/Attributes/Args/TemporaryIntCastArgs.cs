using System;

[Serializable]
public class TemporaryIntCastArgs : TemporaryWrapperCastArgs<int>
{
    public TemporaryIntCastArgs(int duration, bool self, int value) : base(duration, self, value) { }
}