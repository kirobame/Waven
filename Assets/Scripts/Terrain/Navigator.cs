using UnityEngine;
using Flux.Data;

public class Navigator : MonoBehaviour
{
    public float YOffset => Map.Tilemap.layoutGrid.cellSize.y;
    
    public Tile Current { get; private set; }
    public Map Map { get; private set; }
    public Tileable Target => target;
    
    [SerializeReference] private Tileable target;
    
    //------------------------------------------------------------------------------------------------------------------/

    private void Start() => Map = Repository.Get<Map>(References.Map);

    //------------------------------------------------------------------------------------------------------------------/
    
    public void Place(Vector2Int position)
    {
        if (!IsTileValid(position, out var tile)) return;
        
        target.Place(Map.Tilemap.CellToWorld(tile.Position));
        Current = tile;
    }
    public void Move(Tile[] path)
    {
        var positions = new Vector2[path.Length];
        for (var i = 0; i < path.Length; i++) positions[i] = Map.Tilemap.CellToWorld(path[i].Position);

        Current = path[path.Length - 1];
        target.Move(positions);
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    public bool IsTileValid(Vector2Int pos, out Tile tile)
    {
        if (!Map.Tiles.TryGetValue(pos, out tile)) return false;
        return tile.GetType() == typeof(Tile);
    }
}
