using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Sirenix.Utilities;
using UnityEngine;

[Serializable]
public class Cross : Pattern
{
    [SerializeField] protected bool allowBoost;
    [SerializeField] private bool includeSource;
    [SerializeField] private Vector4Int directions;
    
    public override IEnumerable<Tile> GetTiles(Tile source, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var directions = this.directions;
        if (allowBoost && args.TryGet<IWrapper<int>>(new Id('E', 'X', 'T'), out var result))
        {
            directions.down += result.Value;
            directions.up += result.Value;
            directions.left += result.Value;
            directions.right += result.Value;
        }
        
        var output = source.GetCellsInCross(directions).ToHashSet();
        if (!includeSource) output.Remove(source);

        return output;
    }
}