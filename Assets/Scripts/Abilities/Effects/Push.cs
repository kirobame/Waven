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
    [SerializeField] private Vector2Int direction;
    private int business;
    private bool damage;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var force = this.force;
        if (args.TryGetValue(new Id('P', 'S', 'H'), out CastArgs speArgs) && speArgs is IntCastArgs floatCastArgs)
        {
            force += floatCastArgs.Value;
        }
        
        business = 0;
        var targets = tiles.SelectMany(tile => tile.Entities).Where(entity => entity is Player player && player.TeamId != Player.Active.TeamId);

        if (!targets.Any())
        {
            End();
            return;
        }
        
        foreach(var target in targets.ToArray())
        {
            var cell = target.Navigator.Current.FlatPosition + direction;
            if (!cell.TryGetTile(out var sourceTile)) continue;

            var code = sourceTile.GetCellsInLine(force, direction, out var line);
            if (code != 0)
            {
                Debug.Log($"CANNOT PÜSH BECAUSE : {code}");
            }
            
            if (!line.Any())
            {
                Debug.Log("Push Damage");
                continue;
            }

            line.Insert(0, target.Navigator.Current);
            target.Navigator.Move(line.ToArray());
            Routines.Start(WaitForPushEnd(target));
        }
        
        if (business == 0) End();
    }

    private void SetPushPath(ITileable player, IEnumerable<Tile> path)
    {
        var walkableTiles = new List<Tile>();

        for (int i = 1; i < path.Count(); i++)
        {
            if (path.ElementAt(i).IsFree() || (path.ElementAt(i).Entities.Contains(player)))
            {
                damage = false;
                walkableTiles.Add(path.ElementAt(i));
                continue;
            }

            if(path.ElementAt(i) == null)
            {
                damage = true;
                break;
            }

            if (!path.ElementAt(i).IsFree())
            {
                damage = true;
                break;
            }
        }

        player.Navigator.Move(walkableTiles.ToArray());
        Routines.Start(WaitForPushEnd(player));
    }

    private IEnumerator WaitForPushEnd(ITileable player)
    {
        business++;

        while (player.IsMoving) yield return new WaitForSeconds(0.1f);

        if(damage) Debug.Log("Push Damage");

        business--;
        if (business == 0) End();
    }
}
