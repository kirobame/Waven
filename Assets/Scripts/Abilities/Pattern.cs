using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum VectorDirection { left, right, down, up };


public abstract class Pattern
{
    [SerializeField] protected int range;
    public abstract List<WalkableTile> GetTiles(WalkableTile selectedTile);
}

[Serializable]
public class Circle : Pattern
{
    public override List<WalkableTile> GetTiles(WalkableTile selectedTile)
    {
        return selectedTile.GetCellsAround(range).ToList();
    }
}

[Serializable]
public class Line : Pattern
{
    [SerializeField] VectorDirection direction;
    public override List<WalkableTile> GetTiles(WalkableTile selectedTile)
    {
        switch (direction)
        {
            case VectorDirection.left:
                return selectedTile.GetCellsInLigne(range, Vector2Int.left).ToList();
            case VectorDirection.right:
                return selectedTile.GetCellsInLigne(range, Vector2Int.right).ToList();
            case VectorDirection.down:
                return selectedTile.GetCellsInLigne(range, Vector2Int.down).ToList();
            case VectorDirection.up:
                return selectedTile.GetCellsInLigne(range, Vector2Int.up).ToList();
            default:
                return selectedTile.GetCellsInLigne(range, Vector2Int.zero).ToList();
        }
    }
}

[Serializable]
public class Cross : Pattern
{
    public override List<WalkableTile> GetTiles(WalkableTile selectedTile)
    {
        return selectedTile.GetCellsInCross(range).ToList();
    }
}