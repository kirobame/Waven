using Flux;
using Flux.Event;
using UnityEngine;

public class BordermapTrap : Trap
{
    protected override void ApplyOn(ITileable source)
    {

        if (source.TryGet<IDamageable>(out var damageable) && damageable.Team != TeamTag.Neutral) damageable.Inflict(10, DamageType.Base);
        /*
        Debug.Log(source == this);
        if (((Component)source).gameObject.GetComponent<Tag>().Team != TeamTag.Neutral)
            if (source.TryGet<IDamageable>(out var damageable)) damageable.Inflict(10, DamageType.Base);*/
    }
}