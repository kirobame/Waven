using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;

    void Update()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {   
            var localPosition = new Vector3Int(pos.x, pos.y, pos.z);
            var position = tilemap.CellToWorld(localPosition);
            if (tilemap.HasTile(localPosition))
            {
                var tile = tilemap.GetTile(localPosition);
                
                Debug.Log($"Tile at : {localPosition} : {tile.name}");
                Debug.DrawRay(position, Vector3.back * 3.0f, Color.red);
            }
        }
    }
}