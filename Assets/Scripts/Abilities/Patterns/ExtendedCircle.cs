using System;
using System.Collections.Generic;
using Flux;
using Sirenix.Utilities;
using UnityEngine;

[Serializable]
public class ExtendedCircle : ExtendedPattern
{
    [SerializeField] protected bool allowBoost;
    [SerializeField] private bool includeSource;
    [SerializeField] private int radius;
    
    public override IEnumerable<Tile> GetTiles(Tile source, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        var radius = this.radius;
        if (allowBoost && args.TryAggregate(new Id('E', 'X', 'T'), out var result)) radius += result;
    
        var output = source.GetCellsAround(radius).ToHashSet();
        if (!includeSource) output.Remove(source);

        return Trim(output);
    }
}