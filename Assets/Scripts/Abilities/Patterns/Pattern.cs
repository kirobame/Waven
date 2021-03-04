using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Flux;

public abstract class Pattern
{
    public abstract IEnumerable<Tile> GetTiles(Tile source, IReadOnlyDictionary<Id, CastArgs> args);
}