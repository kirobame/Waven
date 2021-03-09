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

    private int business;
    
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        foreach (var entity in tiles.SelectMany(tile => tile.Entities))
        {
            var component = (Component)entity;
            if (!component.TryGetComponent<SequenceRelay>(out var relay)) continue;
            
            var sendback = new SendbackArgs();
            sendback.onDone += OnFeedbackDone;

            if (relay.TryPlay(id, sendback)) business++;
        }
        
        if (business == 0) End();
    }

    void OnFeedbackDone(EventArgs args)
    {
        business--;
        if (business == 0) End();
    }
}