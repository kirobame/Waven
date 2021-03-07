using Flux.Data;
using UnityEngine;

public class WalkableTile : Tile, IMarkable
{
    public WalkableTile(Vector2Int position, int height) : base(position, height) { }
    
    private Mark mark;
    private GameObject marker;
    
    public void Mark(Mark mark)
    {
        if (this.mark != global::Mark.None)
        {
            marker.gameObject.SetActive(false);
            marker = null;
        }

        this.mark = mark;
        if (mark == global::Mark.None) return;

        var pool = Repository.Get<GenericPool>(Pools.HUD);
        var key = Repository.Get<GenericPoolable>(mark);

        marker = pool.CastSingle<GameObject>(key);
        var map = Repository.Get<Map>(References.Map);

        marker.transform.position = map.Tilemap.CellToWorld(Position);
    }
}