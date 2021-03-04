using System;
using System.Collections.Generic;
using Flux;
using UnityEngine;

[Serializable]
public class Circle : Pattern
{
    [SerializeField] private int radius;
    
    public override IEnumerable<Tile> GetTiles(Tile source, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var radius = this.radius;
        if (args.TryGetValue(new Id('E', 'X', 'T'), out var speArgs) && speArgs is IntCastArgs intArgs) radius += intArgs.Value;
        
        return source.GetCellsAround(radius);
    }
}