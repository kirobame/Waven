using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Push : Effect
{
    [SerializeField] int force;

    protected override void ApplyTo(IEnumerable<Tile> tiles)
    {
        var targets = tiles.SelectMany(tile => tile.Entities).Where(entity => entity is Player player && player.TeamId != Player.Active.TeamId);

        foreach(var target in targets) Debug.Log($"Pushing player {force} tiles away");
    }
}
