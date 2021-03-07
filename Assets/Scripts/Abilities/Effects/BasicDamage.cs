using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

[Serializable]
public class BasicDamage : Effect
{
    [SerializeField] private int amount;
    [SerializeField] private DamageType type;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args) 
    {

        foreach (var target in tiles.SelectMany(tile => tile.Entities))
        {
            //if (!target.TryGet<IDamageable>(Player.Active.Team, out var damageable)) continue; ;
            if (!target.TryGet<IDamageable>(out var damageable)) continue; ;
            damageable.Inflict(amount, type);
        }

        End();
    }
}