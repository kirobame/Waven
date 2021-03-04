using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    public Tilemap Tilemap => tilemap;
    public IReadOnlyDictionary<Vector2Int, TileBase> Tiles => tiles;
    
    [SerializeField] private Tilemap tilemap;
    
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
}
