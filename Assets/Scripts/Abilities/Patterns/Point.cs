using System;
using System.Collections.Generic;
using Flux;

[Serializable]
public class Point : Pattern
{
    public override IEnumerable<Tile> GetTiles(Tile source, IReadOnlyDictionary<Id, CastArgs> args) => new Tile[] { source };
}