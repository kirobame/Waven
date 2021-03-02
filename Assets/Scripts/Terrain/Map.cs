using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public Tilemap Tilemap => tilemap;
    public IReadOnlyDictionary<Vector2Int, Tile> Tiles => tiles;
    [SerializeField] private Tilemap tilemap;
    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            var localPosition = new Vector3Int(pos.x, pos.y, pos.z);
            var flattenPosition = new Vector2Int(localPosition.x, localPosition.y);
            //var position = tilemap.CellToWorld(localPosition);
            if (tilemap.HasTile(localPosition))
            {
                var tile = tilemap.GetTile(localPosition);
                var prefix = tile.name.Substring(0, 2).ToUpper();
                Tile implementation = default;

                switch (prefix)
                {
                    case "WK":
                        implementation = new Tile(flattenPosition, 0);
                        break;
                }

                tiles.Add(flattenPosition, implementation);
            }
        }
    }
}
