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
    [SerializeField] private float speed;

    private int business;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var force = this.force;
        if (args.TryGet<IWrapper<int>>(new Id('P', 'S', 'H'), out var result)) force += result.Value;
        
        business = 0;
        var targets = tiles.SelectMany(tile => tile.Entities).Where(entity => entity is Tileable tileable && tileable.Team != Player.Active.Team);

        if (!targets.Any())
        {
            End();
            return;
        }
        
        foreach(var target in targets.ToArray())
        {
            var direction = Vector3.Normalize(target.Navigator.Current.GetWorldPosition() - Player.Active.Navigator.Current.GetWorldPosition());
            var orientation = direction.xy().ComputeOrientation() * (int)Mathf.Sign(force);

            var cell = target.Navigator.Current.FlatPosition + orientation;
            if (!cell.TryGetTile(out var sourceTile)) continue;

            var tuple = sourceTile.GetCellsInLine(Mathf.Abs(force), orientation, out var line);
            if (tuple.code != 0)
            {
                if (target.TryGet<IDamageable>(out var damageable)) damageable.Inflict(1, DamageType.Base);
                if (tuple.code == 3)
                {
                    foreach (var entity in tuple.tile.Entities)
                    {
                        if (entity.TryGet<IDamageable>(out damageable)) damageable.Inflict(1, DamageType.Base);
                    }
                }
            }
            
            if (!line.Any()) continue;

            line.Insert(0, target.Navigator.Current);
            target.Navigator.Move(line.ToArray(), speed, true);
            
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
