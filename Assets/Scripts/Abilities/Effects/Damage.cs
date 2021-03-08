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

    private int business;
    
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args)
    {
        business = 0;
 
        var amount = this.amount; 
        if (args.TryGet<IWrapper<int>>(new Id('D', 'M', 'G'), out var intArgs)) amount += intArgs.Value;
        
        foreach (var target in tiles.SelectMany(tile => tile.Entities))
        {
            if (!target.TryGet<IDamageable>(Player.Active.Team, out var damageable)) continue;

            business++;
            damageable.onFeedbackDone += OnTargetFeedbackDone;
            
            damageable.Inflict(amount, type);
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