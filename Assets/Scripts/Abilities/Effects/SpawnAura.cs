using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class SpawnAura : Effect
{
    [SerializeField] private Aura prefab;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        foreach (var tile in tiles)
            SpawnAt(tile);

        End();
    }
    private void SpawnAt(Tile tile)
    {
        var map = Repository.Get<Map>(References.Map);
        var position = map.Tilemap.CellToWorld(tile.Position);
        foreach (var target in tile.Entities)
        {
            if (!target.TryGet<IDamageable>(out var damageable)) continue; ;
            Aura Aura = Object.Instantiate(prefab, position, Quaternion.identity);
            Aura.Spawn(target.Navigator.GetComponent<Tileable>()); //Try to get cible
        }
    }
}