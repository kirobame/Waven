using System;
using System.Collections.Generic;
using System.Linq;
using Flux.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public static class Extensions
{
    public static Vector2Int xy(this Vector3Int value) => new Vector2Int(value.x, value.y);
    public static Vector3Int Extend(this Vector2Int value) => new Vector3Int(value.x, value.y, 0);

    public static bool TryGetTile(this Vector2Int cell, out WalkableTile walkableTile)
    {
        var map = Repository.Get<Map>(References.Map);
        if (map.Tiles.TryGetValue(cell, out var tile) && tile is WalkableTile output)
        {
            walkableTile = output;
            return true;
        }

        walkableTile = null;
        return false;
    }
    
    public static IEnumerable<WalkableTile> GetCellsAround(this WalkableTile source, int size)
    {
        var list = new List<WalkableTile>();
        
        var startX = source.x - size;
        for (var i = 0; i < size * 2 + 1; i++)
        {
            var x = startX + i;
            
            var height = size - Mathf.Abs(x - source.x);
            var startY = source.y - height;

            for (var j = 0; j < height * 2 + 1; j++)
            {
                var cell = new Vector2Int(x, startY + j);
                if (!cell.TryGetTile(out var tile)) continue;
                
                list.Add(tile);
            }
        }

        return list;
    }

    public static IEnumerable<WalkableTile> GetCellsInLigne(this WalkableTile source, int size, Vector2Int direction)
    {
        var list = new List<WalkableTile>();

        for (var i = 0; i < size * 2 + 1; i++)
        {
            var posX = source.x + direction.x * i;
            var posY = source.y + direction.y * i;

            var cell = new Vector2Int(posX, posY);
            if (!cell.TryGetTile(out var tile)) continue;

            list.Add(tile);
        }

        return list;
    }

    public static IEnumerable<WalkableTile> GetCellsInCross(this WalkableTile source, int size)
    {
        var list = new List<WalkableTile>();


        for (var i = -size; i < size * 2 + 1; i++)
        {
            var posX = source.x + i;

            var cell = new Vector2Int(posX, source.y);
            if (!cell.TryGetTile(out var tile) || list.Contains(tile)) continue;
            list.Add(tile);
        }
        for (var i = -size; i < size * 2 + 1; i++)
        {
            var posY = source.y + i;

            var cell = new Vector2Int(source.x, posY);
            if (!cell.TryGetTile(out var tile) || list.Contains(tile)) continue;
            list.Add(tile);
        }

        return list;
    }

    public static bool IsNeighbourOf(this Tile source, Tile tile)
    {
        var sourcePos = source.Position;
        var tilePos = tile.Position;

        var xDiff = Math.Abs(sourcePos.x - tilePos.x);
        var yDiff = Mathf.Abs(sourcePos.y - tilePos.y);

        return xDiff == 1 && yDiff == 0 || xDiff == 0 && yDiff == 1;
    }
    public static bool IsFree(this WalkableTile tile) => !tile.Entities.Any();

    public static void Mark(this IEnumerable<WalkableTile> tiles, Mark mark)
    {
        foreach (var tile in tiles) tile.SetMark(mark);
    }
}