using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class SpawnTrap : Effect
{
    [SerializeField] private Trap prefab;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args)
    {
        foreach (var tile in tiles)
        {
            if (!tile.Entities.Any(tileable => tileable is Trap)) SpawnAt(tile);
            else
            {
                TryFindSpawnPoint(tile, Vector2Int.down);
                TryFindSpawnPoint(tile, Vector2Int.right);
                TryFindSpawnPoint(tile, Vector2Int.left);
                TryFindSpawnPoint(tile, Vector2Int.up);
            }
        }

        End();
    }

    private void TryFindSpawnPoint(Tile source, Vector2Int direction)
    {
        var match = (source.FlatPosition + direction).TryGetTile(out var tile);
        while (match)
        {
            if (!tile.Entities.Any(tileable => tileable is Trap))
            {
                SpawnAt(tile);
                break;
            }
            
            match = (tile.FlatPosition + direction).TryGetTile(out tile);
        }
    }
    private void SpawnAt(Tile tile)
    {
        var map = Repository.Get<Map>(References.Map);
        
        var position = map.Tilemap.CellToWorld(tile.Position);
        Object.Instantiate(prefab, position, Quaternion.identity);
    }
}