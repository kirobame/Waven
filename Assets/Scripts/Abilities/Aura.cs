using System.Collections;
using System.Collections.Generic;
using Flux.Data;
using UnityEngine;

public class Aura : MonoBehaviour
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
        Vector3Int position = map.Tilemap.WorldToCell(owner.transform.position);


        foreach (var direction in Directions)
        {
            position += direction;
            traps.Add(SpawnTrap(position), direction);
        }
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
        Vector3Int position = map.Tilemap.WorldToCell(owner.transform.position);

        foreach (var trap in traps)
        {
            position += trap.Value;
            trap.Key.Place(map.Tilemap.CellToWorld(position));
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
    public void OnOwnerDestroyed()
    {
        foreach (var trap in traps)
        {
            Destroy(trap.Key.gameObject);
        }
        Destroy(this.gameObject);
    }
    private Trap SpawnTrap(Vector3Int position)
    {
        return Object.Instantiate(prefab, position, Quaternion.identity, this.transform);
    }
}
