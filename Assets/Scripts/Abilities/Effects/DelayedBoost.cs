using System;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using UnityEngine;

[Serializable]
public class DelayedBoost<T> : Effect where T : IMutable
{
    [SerializeField] private Id id;
    [SerializeField] private StatType type;
    [SerializeField] private int value;
    
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        foreach (var tile in tiles)
        {
            foreach (var entity in tile.Entities)
            {
                var component = (Component)entity;
                if (!component.TryGetComponent<IAttributeHolder>(out var attributes)) continue;

                if (!component.TryGetComponent<T>(out var mutable)) continue;
                
                var boost = new DelayedIntCastArgs(value, mutable);
                boost.SetId(id);
                
                var pool = Repository.Get<SequencerPool>(Pools.Popup);
                var popup = pool.RequestSinglePoolable() as Popup;

                popup.transform.position = attributes.PopupAnchor.Position;

                var prefix = value < 0 ? '-' : '+';
                popup.Play($"{prefix}{Mathf.Abs(value)}", type);
                
                attributes.Add(boost);
            }
        }

        End();
    }
}