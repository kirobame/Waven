using System;
using Flux;

public class FallArgs : EventArgs, ISendback
{
    public event Action<EventArgs> onDone;

    public FallArgs(string layer, int order)
    {
        Layer = layer;
        Order = order;
    }
    
    public string Layer { get; private set; }
    public int Order { get; private set; }
    
    public void End(EventArgs args) => onDone?.Invoke(args);
}