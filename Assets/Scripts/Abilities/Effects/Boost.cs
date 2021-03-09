using System;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using UnityEngine;

[Serializable]
public class Boost : Effect
{
    [SerializeField] private CastArgs value;
    [SerializeField] private StatType type;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        foreach (var tile in tiles)
        {
            foreach (var entity in tile.Entities)
            {
                if (!((Component) entity).TryGetComponent<IAttributeHolder>(out var caster)) continue;

                if (value is IWrapper<int> wrapper && wrapper.Value != 0)
                {
                    var pool = Repository.Get<SequencerPool>(Pools.Popup);
                    var popup = pool.RequestSinglePoolable() as Popup;

                    popup.transform.position = caster.PopupAnchor.Position;

                    var prefix = wrapper.Value < 0 ? '-' : '+';
                    popup.Play($"{prefix}{Mathf.Abs(wrapper.Value)}", type);
                }
                caster.Add(value);
            }
        }

        End();
    }
}