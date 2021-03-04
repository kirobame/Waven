using System;
using System.Collections.Generic;
using System.Linq;
using Flux.Data;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public static class Extensions
{
    public static Vector2Int ComputeOrientation(this Vector2 direction)
    {
        if (direction.x < 0 && direction.y > 0) return Vector2Int.left;
        else if (direction.x > 0 && direction.y > 0) return Vector2Int.up;
        else if (direction.x > 0 && direction.y < 0) return Vector2Int.right;
        else return Vector2Int.down;
    }
    public static Vector2Int xy(this Vector3Int value) => new Vector2Int(value.x, value.y);
    public static Vector3Int Extend(this Vector2Int value) => new Vector3Int(value.x, value.y, 0);

    //------------------------------------------------------------------------------------------------------------------/
    
    public static bool TryGetTile(this Vector2Int cell, out Tile tile)
    {
        var map = Repository.Get<Map>(References.Map);
        if (map.Tiles.TryGetValue(cell, out var gottenTile) && gottenTile is Tile output)
        {
            tile = output;
            return true;
        }

        tile = null;
        return false;
    }
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public static IEnumerable<Tile> GetCellsAround(this Tile source, int size)
    {
        var list = new List<Tile>();
        
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
    public static IEnumerable<Tile> GetCellsInLine(this Tile source, int length, Vector2Int direction)
    {
        var list = new List<Tile>();

        for (var i = 0; i < length; i++)
        {
            var cell = source.FlatPosition + direction * i;
            if (!cell.TryGetTile(out var tile)) continue;

            list.Add(tile);
        }

        return list;
    }
    public static byte GetCellsInLine(this Tile source, int length, Vector2Int direction, out List<Tile> output)
    {
        var map = Repository.Get<Map>(References.Map);
        output = new List<Tile>();

        for (var i = 0; i < length; i++)
        {
            var cell = source.FlatPosition + direction * i;
            if (!map.Tiles.TryGetValue(cell, out var gottenTile)) return 1;

            if (!(gottenTile is Tile tile)) return 2;

            if (!tile.IsFree()) return 3;

            output.Add(tile);
        }

        return 0;
    }
    public static IEnumerable<Tile> GetCellsInCross(this Tile source, Vector4Int directions)
    {
        var list = new List<Tile>();
        
        list.AddRange(source.GetCellsInLine(directions.left, Vector2Int.left));
        list.AddRange(source.GetCellsInLine(directions.right, Vector2Int.right));
        list.AddRange(source.GetCellsInLine(directions.up, Vector2Int.up));
        list.AddRange(source.GetCellsInLine(directions.down, Vector2Int.down));

        return list;
    }
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public static bool IsNeighbourOf(this TileBase source, TileBase tileBase)
    {
        var sourcePos = source.Position;
        var tilePos = tileBase.Position;

        var xDiff = Math.Abs(sourcePos.x - tilePos.x);
        var yDiff = Mathf.Abs(sourcePos.y - tilePos.y);

        return xDiff == 1 && yDiff == 0 || xDiff == 0 && yDiff == 1;
    }
    public static bool IsFree(this Tile tile) => !tile.Entities.Any();

    //------------------------------------------------------------------------------------------------------------------/
    
    public static void ClearMarks()
    {
        var map = Repository.Get<Map>(References.Map);
        foreach (var value in map.Tiles.Values)
        {
            if (!(value is Tile tile)) continue;
            tile.Mark(global::Mark.None);
        }
    }
    public static void Mark(this IEnumerable<Tile> tiles, Mark mark)
    {
        foreach (var tile in tiles) tile.Mark(mark);
    }
}