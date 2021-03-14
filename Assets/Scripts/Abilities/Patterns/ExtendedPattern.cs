using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class ExtendedPattern : Pattern
{
    [SerializeField] private CastTarget constraints;
    [SerializeField] private CastTarget removals;

    protected IEnumerable<Tile> Trim(IEnumerable<Tile> tiles)
    {
        IEnumerable<Tile> temp;
        
        if (constraints == CastTarget.None) temp = new List<Tile>(tiles);
        else
        {
            temp = new List<Tile>();
            
            if (constraints.HasFlag(CastTarget.Player)) temp = temp.Concat(tiles.Where(tile => tile.Entities.Any(entity => entity is Player)));
            if (constraints.HasFlag(CastTarget.Golem)) temp = temp.Concat(tiles.Where(tile => tile.Entities.Any(entity => entity is Golem)));
            if (constraints.HasFlag(CastTarget.Trap)) temp = temp.Concat(tiles.Where(tile => tile.Entities.Any(entity => entity.GetType() == typeof(Trap))));
            if (constraints.HasFlag(CastTarget.Verglas)) temp = temp.Concat(tiles.Where(tile => tile.Entities.Any(entity => entity.GetType() == typeof(SlideTrap))));
            if (constraints.HasFlag(CastTarget.Free)) temp = temp.Concat(tiles.Where(tile => tile.IsFree()));
            if (constraints.HasFlag(CastTarget.Neutral)) temp = temp.Concat(tiles.Where(tile => tile.Entities.Any(entity => entity is Tileable tileable && tileable.Team == TeamTag.Neutral)));
            if (constraints.HasFlag(CastTarget.Self)) temp = temp.Concat(tiles.Where(tile => tile.Entities.Any(entity => entity is Tileable tileable && tileable.Team == Player.Active.Team)));
            if (constraints.HasFlag(CastTarget.Enemy)) temp = temp.Concat(tiles.Where(tile => tile.Entities.Any(entity => entity is Tileable tileable && tileable.Team != Player.Active.Team)));
        }

        if (removals == CastTarget.None) return temp;
        
        if (removals.HasFlag(CastTarget.Player)) temp = temp.Where(tile => !tile.Entities.Any(entity => entity is Player));
        if (removals.HasFlag(CastTarget.Golem)) temp = temp.Where(tile => !tile.Entities.Any(entity => entity is Golem));
        if (removals.HasFlag(CastTarget.Trap)) temp = temp.Where(tile => tile.Entities.All(entity => entity.GetType() != typeof(Trap)));
        if (removals.HasFlag(CastTarget.Verglas)) temp = temp.Where(tile => tile.Entities.All(entity => entity.GetType() != typeof(SlideTrap)));
        if (removals.HasFlag(CastTarget.Free)) temp = temp.Where(tile => !tile.IsFree());
        if (removals.HasFlag(CastTarget.Neutral)) temp = temp.Where(tile => !tile.Entities.Any(entity => entity is Tileable tileable && tileable.Team == TeamTag.Neutral));
        if (removals.HasFlag(CastTarget.Self)) temp = temp.Where(tile => !tile.Entities.Any(entity => entity is Tileable tileable && tileable.Team == Player.Active.Team));
        if (removals.HasFlag(CastTarget.Enemy)) temp = temp.Where(tile =>!tile.Entities.Any(entity => entity is Tileable tileable && tileable.Team != Player.Active.Team && tileable.Team != TeamTag.Neutral));

        return temp;
    }
}