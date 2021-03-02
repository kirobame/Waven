using UnityEngine;
using Flux.Data;

public class Navigator : MonoBehaviour
{
    [SerializeReference] public Movable target;
    private Map map;

    private void Start()
    {
        map = Repository.Get<Map>(References.Map);
    }

    public void Place(Vector2Int position)
    {
        if (!IsTileValid(position, out var tile)) return;
        target.Place(map.Tilemap.CellToWorld(tile.Position));
        target.lastPosition = target.transform.position;
    }

    public bool IsTileValid(Vector2Int pos, out Tile tile)
    {
        if(map.Tiles.TryGetValue(pos, out tile))
        {
            return tile.GetType() == typeof(Tile);
        }
        return false;
    }
}
