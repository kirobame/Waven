using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BasicDamage : Effect
{
    [SerializeField] int damages;

    protected override void ApplyTo(IEnumerable<Tile> tiles) 
    {
        var targets = tiles.SelectMany(tile => tile.Entities).Where(entity => entity is Player player && player.TeamId != Player.Active.TeamId);
        foreach (var target in targets) Debug.Log($"Dealing {damages} damage to : {target}");
        
        End();
        Debug.Log("Deal so much fire damages");
    }
}