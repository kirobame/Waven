using System;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Animate : Effect
{
    [SerializeField] private bool ownership;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        var map = Repository.Get<Map>(References.Map);
        foreach (var tile in tiles)
        {
            if (tile.IsFree()) continue;
            foreach (var entity in tile.Entities)
            {
                if (entity.TryGet<Golem>(out var golem)) golem.Activate();


                if (ownership && golem.TryGetComponent<Tag>(out var tag))
                {
                    Debug.Log(golem);
                    tag.Team = Player.Active.Team;
                    Player.Active.AddDependency(golem.gameObject);
                }

            }
            var position = map.Tilemap.CellToWorld(tile.Position);

        }

        End();
    }
}