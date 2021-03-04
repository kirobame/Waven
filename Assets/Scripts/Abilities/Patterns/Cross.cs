using System;
using System.Collections.Generic;
using Flux;
using UnityEngine;

[Serializable]
public class Cross : Pattern
{
    [SerializeField] private Vector4Int directions;
    
    public override IEnumerable<Tile> GetTiles(Tile source, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var directions = this.directions;
        if (args.TryGetValue(new Id('E', 'X', 'T'), out var speArgs) && speArgs is IntCastArgs intArgs)
        {
            directions.down += intArgs.Value;
            directions.up += intArgs.Value;
            directions.left += intArgs.Value;
            directions.right += intArgs.Value;
        }
        
        return source.GetCellsInCross(directions);
    }
}