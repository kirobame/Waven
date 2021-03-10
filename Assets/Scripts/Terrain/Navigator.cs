using UnityEngine;
using Flux.Data;
using Flux.Event;

public class Navigator : MonoBehaviour
{
    public static float YOffset { get; private set; }
    
    public Tile Current { get; private set; }
    //public Map Map { get; private set; }
    
    public ITileable Target => target;
    
    [SerializeReference] private TileableBase target;
    
    //------------------------------------------------------------------------------------------------------------------/

    protected virtual void Start()
    {
        var map = Repository.Get<Map>(References.Map);
        YOffset = map.Tilemap.layoutGrid.cellSize.y;

        var cell = map.Tilemap.WorldToCell(transform.position).xy();
        Place(cell);
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    public void Place(Vector2Int position)
    {
        if (!position.TryGetTile(out var tile)) return;

        var map = Repository.Get<Map>(References.Map);
        target.Place(map.Tilemap.CellToWorld(tile.Position));
        SetCurrent(tile);
    }
    public void Move(Tile[] path, float speed = -1.0f, bool overrideSpeed = false, bool processDir = true)
    {
        var map = Repository.Get<Map>(References.Map);
        
        var positions = new Vector2[path.Length];
        for (var i = 0; i < path.Length; i++) positions[i] = map.Tilemap.CellToWorld(path[i].Position);

        target.Move(positions, speed, overrideSpeed, processDir);
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
