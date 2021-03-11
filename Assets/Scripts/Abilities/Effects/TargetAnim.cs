using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Event;
using UnityEngine;

[Serializable]
public class TargetAnim : Effect
{
    [SerializeField] private Id id;
    [SerializeField] private float duration = 0.75f;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        var targets = tiles.SelectMany(tile => tile.Entities);
        if (!targets.Any())
        {
            End();
            return;
        }
        
        foreach (var entity in targets)
        {
            var component = (Component)entity;
            if (!component.TryGetComponent<SequenceRelay>(out var relay)) continue;
            
            var sendback = new SendbackArgs();
            relay.TryPlay(id, sendback);
        }

        Routines.Start(Routines.DoAfter(End, duration));
    }
}