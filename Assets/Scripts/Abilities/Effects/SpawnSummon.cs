using System;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class SpawnSummon : Effect
{
    [SerializeField] private Summon prefab;

    protected override void ApplyTo(IEnumerable<Tile> tiles)
    {
        var map = Repository.Get<Map>(References.Map);
        foreach (var tile in tiles)
        {
            if (!tile.IsFree()) continue;
            
            var position = map.Tilemap.CellToWorld(tile.Position);
            var instance = Object.Instantiate(prefab, position, Quaternion.identity);
            
            Player.Active.AddDependency(instance.gameObject);
        }

        Routines.Start(Routines.DoAfter(()=> 
        {
            Debug.Log("Summon End");
            End();
            
        }, 1.0f));
    }
}