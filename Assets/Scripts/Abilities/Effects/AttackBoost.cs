using System;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using UnityEngine;

[Serializable]
public class AttackBoost : Effect
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
                if (!component.TryGetComponent<Pathfinder>(out var pathfinder)) continue;

                if (!component.TryGetComponent<IAttributeHolder>(out var attributes)) continue;

                CastArgs boost;
                if (temporary) boost = new TemporaryIntCastArgs(duration, value);
                else boost = new IntCastArgs(value);
                boost.SetId(new Id('A','T','K'));

                /*var pool = Repository.Get<SequencerPool>(Pools.Popup);
                var popup = pool.RequestSinglePoolable() as Popup;

                popup.transform.position = attributes.PopupAnchor.Position;

                var bonus = ((IWrapper<int>)boost).Value;
                var prefix = bonus < 0 ? '-' : '+';
                popup.Play($"{prefix}{Mathf.Abs(bonus)}", StatType.Movement);*/
                
                attributes.Add(boost);
                pathfinder.Dirty();
                
                Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
            }
        }
        
        End();
    }
}