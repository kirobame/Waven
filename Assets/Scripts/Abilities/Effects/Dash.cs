using System;
using System.Collections.Generic;
using Flux;
using UnityEngine;

[Serializable]
public class Dash : Effect
{
    [SerializeField] private int range;
    
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var start = Player.Active.Navigator.Current;
        
        var direction = Vector3.Normalize(source.GetWorldPosition() - start.GetWorldPosition());
        var orientation = direction.xy().ComputeOrientation();

        var list = new List<Tile>();
        list.Add(start);
        
        for (var i = 0; i < range; i++)
        {
            var cell = start.FlatPosition + orientation * (i + 1);
            if (!cell.TryGetTile(out var tile)) break;
            
            list.Add(tile);
            if (tile == source) break;
        }

        if (list.Count <= 1)
        {
            End();
            return;
        }

        Player.Active.onMoveDone += OnMoveDone;
        Player.Active.Navigator.Move(list.ToArray());
    }

    void OnMoveDone()
    {
        Player.Active.onMoveDone -= OnMoveDone;
        End();
    }
}