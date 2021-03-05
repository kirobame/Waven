using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public class Map : MonoBehaviour
{
    public Tilemap Tilemap => tilemap;
    public IReadOnlyDictionary<Vector2Int, TileBase> Tiles => tiles;
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameObject borderTrapPrefab;
    [SerializeField] private GameObject borderObstaclePrefab;

    private Dictionary<Vector2Int, TileBase> tiles = new Dictionary<Vector2Int, TileBase>();

    //------------------------------------------------------------------------------------------------------------------/
    
    void Awake()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            var localPosition = new Vector3Int(pos.x, pos.y, pos.z);
            var flatPosition = new Vector2Int(localPosition.x, localPosition.y);

            if (tilemap.HasTile(localPosition))
            {
                var tile = tilemap.GetTile(localPosition);
                var prefix = tile.name.Substring(0, 2).ToUpper();
                TileBase implementation = default;

                switch (prefix)
                {
                    case "WK":
                        implementation = new Tile(flatPosition, 0);
                        break;
                }

                tiles.Add(flatPosition, implementation);
            }
        }
    }


    public void SpawnBordermap()
    {
        var Directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        var borderCells = new List<Vector2Int>();

        foreach (var cell in tiles.Keys)
        {
            foreach (var direction in Directions)
            {
                var neighbour = cell + direction;
                if (!neighbour.IsValidTile())
                {
                    if (!borderCells.Contains(neighbour))
                        borderCells.Add(neighbour);
                }
            }
        }

        foreach (var cell in borderCells)
        {
            var position = this.Tilemap.CellToWorld((Vector3Int)cell);
            Object.Instantiate(borderTrapPrefab, position, Quaternion.identity);
            Object.Instantiate(borderObstaclePrefab, position, Quaternion.identity);
        }
    }
}
