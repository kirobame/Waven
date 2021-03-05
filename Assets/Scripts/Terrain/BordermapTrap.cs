using Flux;
using Flux.Event;
using UnityEngine;

public class BordermapTrap : Trap
{
    protected override void ApplyOn(ITileable source)
    {
        if (((Component)source).gameObject.GetComponent<Tag>().Team != TeamTag.Neutral)
            if (source.TryGet<IDamageable>(out var damageable)) damageable.Inflict(10, DamageType.Base);
    }
}