using UnityEngine;
using Flux.Data;

public class Navigator : MonoBehaviour
{
    public static float YOffset { get; private set; }
    
    public WalkableTile Current { get; private set; }
    public Map Map { get; private set; }
    
    public Tileable Target => target;
    
    [SerializeReference] private Tileable target;
    
    //------------------------------------------------------------------------------------------------------------------/

    private void Start()
    {
        Map = Repository.Get<Map>(References.Map);
        YOffset = Map.Tilemap.layoutGrid.cellSize.y;
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    public void Place(Vector2Int position)
    {
        if (!position.TryGetTile(out var tile)) return;
        
        target.Place(Map.Tilemap.CellToWorld(tile.Position));
        SetCurrent(tile);
    }
    public void Move(WalkableTile[] path)
    {
        var positions = new Vector2[path.Length];
        for (var i = 0; i < path.Length; i++) positions[i] = Map.Tilemap.CellToWorld(path[i].Position);

        SetCurrent(path[path.Length - 1]);
        target.Move(positions);
    }

    private void SetCurrent(WalkableTile tile)
    {
        Current?.Unregister(target);
        
        Current = tile;
        tile.Register(target);
    }
}
