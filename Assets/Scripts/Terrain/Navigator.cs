using UnityEngine;
using Flux.Data;

public class Navigator : MonoBehaviour
{
    public Tile Current { get; private set; }
    public Map Map { get; private set; }
    
    [SerializeReference] public Movable target;

    private void Start() => Map = Repository.Get<Map>(References.Map);

    public void Place(Vector2Int position)
    {
        if (!IsTileValid(position, out var tile)) return;
        
        target.Place(Map.Tilemap.CellToWorld(tile.Position));
        Current = tile;
    }

    public bool IsTileValid(Vector2Int pos, out Tile tile)
    {
        if (!Map.Tiles.TryGetValue(pos, out tile)) return false;
        return tile.GetType() == typeof(Tile);
    }
}
