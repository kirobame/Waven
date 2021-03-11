using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using UnityEngine;

[Serializable]
public class VfxEffect : Effect
{
    [SerializeField] private VfxKey key;

    private int business;
        
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        if (!tiles.Any()) End();

        business = 0;
        var pool = Repository.Get<VfxPool>(Pools.Vfx);
        
        foreach (var tile in tiles)
        {
            var instance = pool.RequestSinglePoolable(GetKey());

            business++;
            instance.onDone += OnEffectDone;
            
            instance.transform.position = tile.GetWorldPosition();
            instance.Play();
        }
    }

    protected virtual PoolableVfx GetKey()
    {
        var key = Repository.Get<VfxGiver>(this.key);
        return key.Get(EventArgs.Empty);
    }
    
    void OnEffectDone(PoolableVfx vfx)
    {
        vfx.onDone -= OnEffectDone;

        business--;
        if (business <= 0) End();
    }
}