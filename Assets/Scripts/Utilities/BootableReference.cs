using Flux;
using Flux.Data;

public class BootableReference : SingleReference
{
    protected override void Awake()
    {
        base.Awake();
        if (Value is IBootable bootable) bootable.Bootup();
    }
}