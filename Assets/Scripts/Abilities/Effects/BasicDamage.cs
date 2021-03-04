using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

[Serializable]
public class BasicDamage : Effect
{
    [SerializeField] int damages;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args) 
    {
        var targets = tiles.SelectMany(tile => tile.Entities).Where(entity => entity is Player player && player.TeamId != Player.Active.TeamId);
        foreach (var target in targets) Debug.Log($"Dealing {damages} damage to : {target}");
        
        End();
        Debug.Log("Deal so much fire damages");
    }
}