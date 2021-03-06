using System.Collections.Generic;
using UnityEngine;

public class Tile : TileBase
{
    public Tile(Vector2Int position, int height) : base (position, height) { }

    public IReadOnlyList<ITileable> Entities => entities;
    private List<ITileable> entities = new List<ITileable>();
    
    public virtual void Register(ITileable entity) => entities.Add(entity);
    public virtual void Unregister(ITileable entity) => entities.Remove(entity);
}