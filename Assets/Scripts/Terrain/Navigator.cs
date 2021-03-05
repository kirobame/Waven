using UnityEngine;
using Flux.Data;
using Flux.Event;

public class Navigator : MonoBehaviour
{
    public static float YOffset { get; private set; }
    
    public Tile Current { get; private set; }
    public Map Map { get; private set; }
    
    public ITileable Target => target;
    
    [SerializeReference] private TileableBase target;
    
    //------------------------------------------------------------------------------------------------------------------/

    private void Start()
    {
        Map = Repository.Get<Map>(References.Map);
        YOffset = Map.Tilemap.layoutGrid.cellSize.y;

        var cell = Map.Tilemap.WorldToCell(transform.position).xy();
        Place(cell);
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    public void Place(Vector2Int position)
    {
        if (!position.TryGetTile(out var tile)) return;

        target.Place(Map.Tilemap.CellToWorld(tile.Position));
        SetCurrent(tile);
    }
    public void Move(Tile[] path, float speed = -1.0f, bool overrideSpeed = false)
    {
        var positions = new Vector2[path.Length];
        for (var i = 0; i < path.Length; i++) positions[i] = Map.Tilemap.CellToWorld(path[i].Position);

        target.Move(positions, speed, overrideSpeed);
    }

    public void SetCurrent(Tile tile)
    {
        RemoveFromBoard();
        
        Current = tile;
        tile.Register(target);
        
        Events.ZipCall(GameEvent.OnTileChange, Target);
    }
    public void RemoveFromBoard() => Current?.Unregister(target);
}
