using System.Collections;
using System.Linq;
using System.Collections.Generic;

public abstract class Pattern
{
    public abstract IEnumerable<Tile> GetTiles(Tile source);
}