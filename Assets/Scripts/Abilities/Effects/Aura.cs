using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using UnityEngine;

[Serializable]
public class Aura : Effect
{
    [SerializeField] private int amount;
    [SerializeField] private DamageType type;
    
    private int business;
    
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        var directions = new Vector2Int[]
        {
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.right
        };
        
        var pool = Repository.Get<VfxPool>(Pools.Vfx);
        foreach (var target in tiles.SelectMany(tile => tile.Entities))
        {
            if (!(target is Golem golem)) continue;

            var poolableKey = Repository.Get<VfxGiver>(VfxKey.Aura).Get(EventArgs.Empty);
            var instance = pool.RequestSinglePoolable(poolableKey);

            instance.transform.position = target.Navigator.Current.GetWorldPosition();
            instance.Play();
            
            foreach (var direction in directions)
            {
                var cell = target.Navigator.Current.FlatPosition + direction;
                if (!cell.TryGetTile(out var tile)) continue;

                foreach (var subTarget in tile.Entities)
                {
                    if (!subTarget.TryGet<IDamageable>(out var damageable) || !damageable.IsAlive) continue;
                    
                    business++;
                    damageable.onFeedbackDone += OnTargetFeedbackDone;
            
                    damageable.Inflict(amount, type);
                }
            }
        }
        
        if (business == 0) End();
    }
    
    void OnTargetFeedbackDone(IDamageable damageable)
    {
        damageable.onFeedbackDone -= OnTargetFeedbackDone;
        
        business--;
        if (business == 0) End();
    }
}