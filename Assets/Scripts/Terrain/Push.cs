using Flux;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Push : Effect
{
    [SerializeField] int force;
    private int business;

    protected override void ApplyTo(IEnumerable<Tile> tiles)
    {
        business = 0;
        var targets = tiles.SelectMany(tile => tile.Entities).Where(entity => entity is Player player && player.TeamId != Player.Active.TeamId);

        if (!targets.Any()) End();

        foreach(var target in targets.ToArray())
        {
            var line = target.Navigator.Current.GetCellsInLine(force, Vector2Int.left);

            if (line.Count() <= 1) continue;
            Debug.Log("Push");

            target.Navigator.Move(line.ToArray());
            Routines.Start(WaitForPushEnd(target)); 
        }
    }

    private IEnumerator WaitForPushEnd(ITileable player)
    {
        business++;
        while (player.IsMoving)
        {
            yield return new WaitForSeconds(0.1f);
        }

        business--;

        if (business == 0)
        {
            End();
            Debug.Log("PushEnd");
        }   
    }
}
