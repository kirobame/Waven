using System;
using UnityEngine;

[Serializable]
public class SpawnSlideTrap : SpawnTrap
{
    protected override Trap SpawnAt(Tile tile, Vector2Int direction)
    {
        var trap = base.SpawnAt(tile, direction);
        if (trap is SlideTrap slideTrap) slideTrap.SlideDirection = direction;
        
        return trap;
    }
}