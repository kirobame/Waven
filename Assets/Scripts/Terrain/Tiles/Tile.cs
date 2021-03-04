﻿using System.Collections.Generic;
using Flux.Data;
using UnityEngine;

public class Tile : TileBase
{
    public Tile(Vector2Int position, int height) : base (position, height) { }

    public IReadOnlyList<ITileable> Entities => entities;
    private List<ITileable> entities = new List<ITileable>();
    
    private Mark mark;
    private GameObject marker;

    public void Register(ITileable entity) => entities.Add(entity);
    public void Unregister(ITileable entity) => entities.Remove(entity);
    
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