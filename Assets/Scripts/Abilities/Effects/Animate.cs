using System;
using System.Collections.Generic;
using Flux;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Animate : Effect
{
    [SerializeField] private bool ownership;
    [SerializeField] private int activation;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        foreach (var tile in tiles)
        {
            if (tile.IsFree()) continue;
            
            foreach (var entity in tile.Entities)
            {
                if (!entity.TryGet<Golem>(out var golem)) continue;
                
                golem.activation += activation;
                golem.LinkTo(Player.Active);
                    
                if (ownership && golem.TryGetComponent<Tag>(out var tag)) tag.Team = Player.Active.Team;
            }
        }

        End();
    }
}