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

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var force = this.force;
        if (args.TryGetValue(new Id('P', 'S', 'H'), out CastArgs speArgs) && speArgs is IntCastArgs floatCastArgs)
        {
            force += floatCastArgs.Value;
        }
        
        business = 0;
        var targets = tiles.SelectMany(tile => tile.Entities).Where(entity => entity is Tileable tileable && tileable.Team != Player.Active.Team);

        if (!targets.Any())
        {
            End();
            return;
        }
        
        foreach(var target in targets.ToArray())
        {
            var cell = target.Navigator.Current.FlatPosition + direction;
            if (!cell.TryGetTile(out var sourceTile)) continue;

            var tuple = sourceTile.GetCellsInLine(force, direction, out var line);
            if (tuple.code != 0)
            {
                if (tuple.code == 3)
                {
                    if (target.TryGet<IDamageable>(out var damageable)) damageable.Inflict(1, DamageType.Base);
                    foreach (var entity in tuple.tile.Entities)
                    {
                        if (entity.TryGet<IDamageable>(out damageable)) damageable.Inflict(1, DamageType.Base);
                    }
                }
            }
            
            if (!line.Any()) continue;

            line.Insert(0, target.Navigator.Current);
            target.Navigator.Move(line.ToArray());
            
            Routines.Start(WaitForPushEnd(target));
        }
        
        if (business == 0) End();
    }

    private IEnumerator WaitForPushEnd(ITileable player)
    {
        business++;

        while (player.IsMoving) yield return new WaitForSeconds(0.1f);

        business--;
        if (business == 0) End();
    }
}
