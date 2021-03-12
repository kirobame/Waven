using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

[Serializable]
public class Damage : Effect
{
    [SerializeField] private int amount;
    [SerializeField] private DamageType type;
    [SerializeField] private bool ownership;

    private int business;
    
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        business = 0;

        var amount = this.amount;
        if (args.TryAggregate(new Id('D', 'M', 'G'), out var output)) amount += output;
        
        if (amount <= 0)
        {
            //Debug.Log($"No damage to do");
            
            End();
            return;
        }
        
        foreach (var target in tiles.SelectMany(tile => tile.Entities))
        {
            if (!TryGetDamageable(target, out var damageable) || !damageable.IsAlive) continue;

            business++;
            damageable.onFeedbackDone += OnTargetFeedbackDone;
            
            damageable.Inflict(amount, type);
        }
        
        if (business == 0) End();
    }

    private bool TryGetDamageable(ITileable target, out IDamageable damageable)
    {
        if (ownership && target.TryGet<IDamageable>(Player.Active.Team, out damageable)) return true;
        else if (!ownership && target.TryGet<IDamageable>(out damageable)) return true;
        else
        {
            damageable = null;
            return false;
        }
    }
    
    void OnTargetFeedbackDone(IDamageable damageable)
    {
        damageable.onFeedbackDone -= OnTargetFeedbackDone;
        
        business--;
        if (business == 0) End();
    }
}