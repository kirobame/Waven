using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Flux;
using UnityEngine;

public abstract class Pattern
{
    public abstract IEnumerable<Tile> GetTiles(Tile source, IReadOnlyDictionary<Id, CastArgs> args);
}