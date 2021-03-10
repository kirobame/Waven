﻿using System;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Spawn : Effect
{
    [SerializeField] private TileableBase prefab;
    [SerializeField] private bool ownership;
    [SerializeField] private bool link;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        var map = Repository.Get<Map>(References.Map);
        foreach (var tile in tiles)
        {
            if (!tile.IsFree()) continue;
            
            var position = map.Tilemap.CellToWorld(tile.Position);
            var instance = Object.Instantiate(prefab, position, Quaternion.identity);

            instance.Navigator.Place(tile.FlatPosition);
            
            if (ownership && instance.TryGetComponent<Tag>(out var tag)) tag.Team = Player.Active.Team;
            if (link) Player.Active.AddDependency(instance.gameObject);
        }

        End();
    }
}