using System;
using System.Collections.Generic;
using Flux;
using UnityEngine;

[Serializable]
public class Boost : Effect
{
    [SerializeField] private CastArgs value;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args)
    {
        foreach (var tile in tiles)
        {
            foreach (var entity in tile.Entities)
            {
                if (!((Component) entity).TryGetComponent<IAttributeHolder>(out var caster)) continue;
                caster.Add(value);
            }
        }

        End();
    }
}