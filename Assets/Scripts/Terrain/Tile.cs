using System.Collections;
using System.Collections.Generic;
using Flux.Data;
using UnityEngine;

public class Tile
{
    public Tile(Vector2Int pos, int height) 
    {
        position = pos;
        this.height = height;
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    public Vector2Int FlatPosition => position;
    public int x => position.x;
    public int y => position.y;
    
    public Vector3Int Position => new Vector3Int(position.x, position.y, height);

    private Mark mark;
    private GameObject marker;
    
    private Vector2Int position;
    private int height;
    
    //------------------------------------------------------------------------------------------------------------------/

    public void SetMark(Mark mark)
    {
        if (this.mark != Mark.None)
        {
            marker.gameObject.SetActive(false);
            marker = null;
        }

        this.mark = mark;
        if (mark == Mark.None) return;

        var pool = Repository.Get<GenericPool>(Pools.HUD);
        var key = Repository.Get<GenericPoolable>(mark);

        marker = pool.CastSingle<GameObject>(key);
        var map = Repository.Get<Map>(References.Map);

        marker.transform.position = map.Tilemap.CellToWorld(Position);
    }
}
