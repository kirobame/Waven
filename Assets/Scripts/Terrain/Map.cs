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
                Tile implementation = default;

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
    
    //------------------------------------------------------------------------------------------------------------------/

    public void MarkRange(Tile center, int size, Mark mark)
    {
        var startX = center.x - size;
        for (var i = 0; i < size * 2 + 1; i++)
        {
            var x = startX + i;
            
            var height = size - Mathf.Abs(x - center.x);
            var startY = center.y - height;

            for (var j = 0; j < height * 2 + 1; j++)
            {
                var cell = new Vector2Int(x, startY + j);
                if (!tiles.TryGetValue(cell, out var tile)) continue;
                
                tile.SetMark(mark);
            }
        }
    }
}
