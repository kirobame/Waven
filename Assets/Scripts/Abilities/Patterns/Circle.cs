using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Sirenix.Utilities;
using UnityEngine;

[Serializable]
public class Circle : Pattern
{
    [SerializeField] protected bool allowBoost;
    [SerializeField] private bool includeSource;
    [SerializeField] private int radius;
    
    public override IEnumerable<Tile> GetTiles(Tile source, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var radius = this.radius;
        if (allowBoost && args.TryGet<IWrapper<int>>(new Id('E', 'X', 'T'), out var result)) radius += result.Value;

        
        var output = source.GetCellsAround(radius).ToHashSet();
        if (!includeSource) output.Remove(source);

        return output;
    }
}