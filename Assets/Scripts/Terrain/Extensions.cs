using System;
using UnityEngine;

public static class Extensions
{
    public static Vector2Int xy(this Vector3Int value) => new Vector2Int(value.x, value.y);
    public static Vector3Int Extend(this Vector2Int value) => new Vector3Int(value.x, value.y, 0);

    public static bool IsNeighbourOf(this Tile source, Tile tile)
    {
        var sourcePos = source.Position;
        var tilePos = tile.Position;

        var xDiff = Math.Abs(sourcePos.x - tilePos.x);
        var yDiff = Mathf.Abs(sourcePos.y - tilePos.y);

        return xDiff == 1 && yDiff == 0 || xDiff == 0 && yDiff == 1;
    }
}