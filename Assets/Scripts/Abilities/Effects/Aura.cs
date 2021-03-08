using System.Collections;
using System.Collections.Generic;
using Flux.Data;
using UnityEngine;

public class Aura : TileableBase
{
    Tileable owner;
    
    [SerializeField] Trap prefab;
    
    Dictionary<Trap, Vector3Int> traps = new Dictionary<Trap, Vector3Int>();

    public void Spawn(Tileable _owner)
    {
        var Directions = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };
        var map = Repository.Get<Map>(References.Map);

        owner = _owner;
        foreach (var direction in Directions)
        {
            var cell = map.Tilemap.WorldToCell(owner.transform.position);
            cell += direction;

            if (!cell.xy().TryGetTile(out var tile)) continue;
            traps.Add(SpawnTrap(tile.GetWorldPosition()), direction);
        }

        owner.onMoveStart += Deactivate;
        owner.onMoveDone += Activate;
        owner.onDestroy += OnOwnerDestroyed;
    }
    public void Activate(ITileable tileable)
    {
        var Directions = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };
        var map = Repository.Get<Map>(References.Map);

        foreach (var trap in traps)
        {
            var cell = map.Tilemap.WorldToCell(owner.transform.position);
            cell += trap.Value;

            if (!cell.xy().TryGetTile(out var tile))
            {
                trap.Key.Navigator.RemoveFromBoard();
                trap.Key.gameObject.SetActive(false);
                
                continue;
            }

            trap.Key.Place(tile.FlatPosition);
            trap.Key.gameObject.SetActive(true);
        }
    }
    public void Deactivate(ITileable tileable) { foreach (var trap in traps) trap.Key.gameObject.SetActive(false); }
    
    public void OnOwnerDestroyed(ITileable tileable)
    {
        foreach (var trap in traps)
        {
            if (!trap.Key) continue;
            Destroy(trap.Key.gameObject);
        }
        
        DestroyAura();
    }
    
    private Trap SpawnTrap(Vector2 position) => Instantiate(prefab, position, Quaternion.identity, transform);

    public void DestroyAura()
    {
        if (owner)
        {
            owner.onMoveStart -= Deactivate;
            owner.onMoveDone -= Activate;
            owner.onDestroy -= OnOwnerDestroyed;
        }
        
        Destroy(this);
    }
}
