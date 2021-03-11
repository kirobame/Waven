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

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        foreach (var tile in tiles)
        {
            if (!tile.Entities.Any(tileable => tileable is Trap))
            {
                var direction = Vector3.Normalize(source.GetWorldPosition() - Player.Active.Navigator.Current.GetWorldPosition());
                SpawnAt(tile, direction.xy().ComputeOrientation());
            }
            else if (tile.Entities.Any(tileable => tileable.GetType() == prefab.GetType()))
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
        if (!(source.FlatPosition + direction).TryGetTile(out var tile) || tile.Entities.Any(tileable => tileable is Trap && tileable.GetType() != prefab.GetType())) return;

        if (!(tile is DeathTile) && !tile.Entities.Any(tileable => tileable is Trap)) SpawnAt(tile, direction);
    }
    protected virtual Trap SpawnAt(Tile tile, Vector2Int direction)
    {
        var map = Repository.Get<Map>(References.Map);
        
        var position = map.Tilemap.CellToWorld(tile.Position);
        return Object.Instantiate(prefab, position, Quaternion.identity);
    }
}