using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using UnityEngine;

[Serializable]
public abstract class Boost : Effect
{
    public abstract StatType Type { get; }

    [SerializeField] private float delay;
    
    public abstract CastArgs GetBoost();
    
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        Routines.Start(Routine(tiles));
    }
    private IEnumerator Routine(IEnumerable<Tile> tiles)
    {
        yield return new WaitForSeconds(delay);
        
        foreach (var tile in tiles)
        {
            foreach (var entity in tile.Entities)
            {
                if (!((Component)entity).TryGetComponent<IAttributeHolder>(out var caster)) continue;
                HandleTarget(entity, caster);
            }
        }

        End();
    }

    protected virtual void HandleTarget(ITileable target, IAttributeHolder attributes)
    {
        var boost = GetBoost();
        if (boost is IWrapper<int> wrapper && wrapper.Value != 0)
        {
            var pool = Repository.Get<SequencerPool>(Pools.Popup);
            var popup = pool.RequestSinglePoolable() as Popup;

            popup.transform.position = attributes.PopupAnchor.Position;

            var prefix = wrapper.Value < 0 ? '-' : '+';
            popup.Play($"{prefix}{Mathf.Abs(wrapper.Value)}", Type);
        }
                
        attributes.Add(boost);
    }
}