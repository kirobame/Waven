using System;
using System.Collections.Generic;
using System.Linq;
using Flux;

[Serializable]
public class GiveAura : Effect
{
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        foreach (var entity in tiles.SelectMany(tile => tile.Entities))
        {
            if (!(entity is Golem golem)) continue;
            golem.HasAura = true;
          
        }
        
        End();
    }
}