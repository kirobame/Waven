﻿using Flux;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux.Event;
using UnityEngine;

[Serializable]
public class Push : Effect
{
    [SerializeField] int force;
    [SerializeField] private int direction;
    [SerializeField] private float speed;

    private int business;

    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        var force = this.force;
        if (args.TryAggregate(new Id('P', 'S', 'H'), out var output)) force += output;
        force *= direction;

        if (force == 0)
        {
            End();
            return;
        }
        
        business = 0;
        var targets = tiles.SelectMany(tile => tile.Entities).Where(entity => entity is Tileable tileable && tileable.Team != Player.Active.Team);

        if (!targets.Any())
        {
            End();
            return;
        }

        foreach (var target in targets)
        {
            var hasDamageable = false;
            if (target.TryGet<IDamageable>(out var damageable))
            {
                if (!damageable.IsAlive) continue;
                hasDamageable = true;
            }

            var direction = Vector3.Normalize(target.Navigator.Current.GetWorldPosition() - Player.Active.Navigator.Current.GetWorldPosition());
            var orientation = direction.xy().ComputeOrientation() * (int)Mathf.Sign(force);
            target.SetOrientation(-orientation);
            
            var cell = target.Navigator.Current.FlatPosition + orientation;
            if (!cell.TryGetTile(out var start)) continue;

            var result = start.GetCellsInLine(Mathf.Abs(force), orientation, out var line);
            if (result.code != 0 && !line.Any())
            {
                if (hasDamageable) RegisterDamageable(damageable);
                
                if (result.code == 3)
                {
                    foreach (var entity in result.tile.Entities)
                    {
                        if (!entity.TryGet<IDamageable>(out damageable)) continue;
                        RegisterDamageable(damageable);
                    }
                }

                continue;
            }
            
            line.Insert(0, target.Navigator.Current);
            if (result.code == 3) line.Add(result.tile);
            
            Events.ZipCall(ChallengeEvent.OnPush, target);
            target.Navigator.Move(line.ToArray(), speed, true, false);

            business++;
            target.onMoveDone += OnMoveEnd;
        }

        if (business == 0) End();
    }

    void OnMoveEnd(ITileable tileable)
    {
        tileable.onMoveDone -= OnMoveEnd;
        
        business--;
        if (business == 0) End();
    }

    private void RegisterDamageable(IDamageable damageable)
    {
        business++;
        damageable.onFeedbackDone += OnDamageDone;
                        
        damageable.Inflict(1, DamageType.Base);
    }
    void OnDamageDone(IDamageable damageable)
    {
        damageable.onFeedbackDone -= OnDamageDone;
        
        business--;
        if (business == 0) End();
    }
}
