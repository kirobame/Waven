using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public class Map : MonoBehaviour
{
    #region Nested Types

    [Serializable]
    private class BorderPrefab
    {
        public GameObject Value => value;
        [SerializeField] private GameObject value;

        public Vector2Int Direction => direction;
        [SerializeField] private Vector2Int direction;
    }

    #endregion
    
    public Tilemap Tilemap => tilemap;
    public IReadOnlyDictionary<Vector2Int, TileBase> Tiles => tiles;
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private BorderPrefab[] borderPrefabs;

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
                        implementation = new WalkableTile(flatPosition, 0);
                        break;
                }

                tiles.Add(flatPosition, implementation);
            }
        }
    }


    public void SpawnBordermap()
    {
        var keys = tiles.Keys.ToArray();
        foreach (var cell in keys)
        {
            foreach (var borderPrefab in borderPrefabs)
            {
                var neighbour = cell + borderPrefab.Direction;
                if (neighbour.IsValidTile() || tiles.ContainsKey(neighbour)) continue;
                
                tiles.Add(neighbour, new DeathTile(neighbour, 0));
                Instantiate(borderPrefab.Value, tilemap.CellToWorld(neighbour.Extend()), Quaternion.identity);
            }
        }
    }
}
