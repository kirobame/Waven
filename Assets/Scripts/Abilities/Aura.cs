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
            Vector3Int cell = map.Tilemap.WorldToCell(owner.transform.position);
            cell += direction;
            Vector2 position = map.Tilemap.CellToWorld(cell);
            traps.Add(SpawnTrap(position), direction);
        }

        owner.onMoveStart += Deactive;
        owner.onMoveDone += Active;
        owner.onDestroy += OnOwnerDestroyed;
    }
    public void Active()
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
            Vector3Int cell = map.Tilemap.WorldToCell(owner.transform.position);
            cell += trap.Value;
            Vector2 position = map.Tilemap.CellToWorld(cell);
            trap.Key.Place(position);
            trap.Key.gameObject.SetActive(true);
        }
    }
    public void Deactive()
    {
        foreach (var trap in traps)
        {
            trap.Key.gameObject.SetActive(false);
        }
    }
    public void OnOwnerDestroyed(ITileable tileable)
    {
        foreach (var trap in traps)
        {
            if (trap.Key)
                Destroy(trap.Key.gameObject);
        }
        DestroyAura();
    }
    private Trap SpawnTrap(Vector2 position)
    {
        return Object.Instantiate(prefab, position, Quaternion.identity, this.transform);
    }

    public void DestroyAura()
    {
        if (owner)
        {
            owner.onMoveStart -= Deactive;
            owner.onMoveDone -= Active;
            owner.onDestroy -= OnOwnerDestroyed;
        }
        Destroy(this);
    }
}
