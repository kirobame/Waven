using System;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;

[Serializable]
public class MovementBoost : Effect
{
    [SerializeField] private int value;
    [SerializeField] private bool temporary;
    [SerializeField] private int duration;
    
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        foreach (var tile in tiles)
        {
            foreach (var entity in tile.Entities)
            {
                var component = (Component)entity;
                if (!component.TryGetComponent<Moveable>(out var moveable)) continue;

                if (!component.TryGetComponent<IAttributeHolder>(out var attributes)) continue;
                
                CastArgs boost;
                if (temporary) boost = new TemporaryIntCastArgs(duration, value);
                else boost = new IntCastArgs(value);
                boost.SetId(new Id('M','V','T'));

                var pool = Repository.Get<SequencerPool>(Pools.Popup);
                var popup = pool.RequestSinglePoolable() as Popup;

                popup.transform.position = attributes.PopupAnchor.Position;

                var number = ((IWrapper<int>)boost).Value;
                var prefix = number < 0 ? '-' : '+';
                popup.Play($"{prefix}{Mathf.Abs(number)}", StatType.Movement);
                
                attributes.Add(boost);
                moveable.Dirty();
                
                Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
            }
        }
        
        End();
    }
}