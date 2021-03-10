using System;
using System.Collections.Generic;
using System.Linq;
using Flux;

[Serializable]
public class Inversion : Effect
{
    private ITileable target;
    
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        var entities = tiles.SelectMany(tile => tile.Entities);
        if (!entities.Any())
        {
            End();
            return;
        }

        target = entities.First();
        Routines.Start(Routines.DoAfter(Execute, 0.75f));
    }

    private void Execute()
    {
        var oldTile = Player.Active.Navigator.Current;
        Player.Active.Navigator.Place(target.Navigator.Current.FlatPosition);
        target.Navigator.Place(oldTile.FlatPosition);
        
        End();
    }
}