using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using UnityEngine;

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
        Routines.Start(Execute());
    }

    private IEnumerator Execute()
    {
        yield return new WaitForSeconds(0.5f);
        
        var pool = Repository.Get<VfxPool>(Pools.Vfx);
        var key = Repository.Get<VfxGiver>(VfxKey.Inversion).Get(EventArgs.Empty);
        
        SpawnVfx(pool, key, Player.Active.Navigator.Current);
        SpawnVfx(pool, key, target.Navigator.Current);
        
        yield return new WaitForSeconds(0.25f);
        
        var oldTile = Player.Active.Navigator.Current;
        Player.Active.Navigator.Place(target.Navigator.Current.FlatPosition);
        target.Navigator.Place(oldTile.FlatPosition);
        
        End();
    }

    private void SpawnVfx(VfxPool pool, PoolableVfx key, Tile tile)
    {
        var instance = pool.RequestSinglePoolable(key);
        instance.transform.position = tile.GetWorldPosition();
        instance.Play();
    }
}