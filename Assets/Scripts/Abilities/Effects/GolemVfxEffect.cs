using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using UnityEngine;

[Serializable]
public class GolemVfxEffect : Effect
{
    [SerializeField] private VfxKey key;

    private int business;
        
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        if (!tiles.Any()) End();

        business = 0;
        var pool = Repository.Get<VfxPool>(Pools.Vfx);

        var any = false;
        foreach (var tile in tiles)
        {
            if (!tile.Entities.Any(entity => entity is Golem)) continue;

            any = true;
            var instance = pool.RequestSinglePoolable(GetKey());

            business++;
            instance.onDone += OnEffectDone;
            
            instance.transform.position = tile.GetWorldPosition();
            instance.Play();
        }
        
        if (!any) End();
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