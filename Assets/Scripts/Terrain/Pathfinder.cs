using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Flux.Event;
using Sirenix.Utilities;

public class Pathfinder : MonoBehaviour, IActivable
{
    [SerializeField] private Navigator nav;
    [SerializeField, Range(0,5)] private int range;

    private WalkableTile Current => path[path.Count - 1];
    private List<WalkableTile> path = new List<WalkableTile>();

    private HashSet<WalkableTile> availableTiles = new HashSet<WalkableTile>();
    private bool isActive;
 
    //------------------------------------------------------------------------------------------------------------------/
    
    public void Activate()
    {
        Events.RelayByValue<WalkableTile>(InputEvent.OnTileSelected, OnTileSelected);
        Events.RelayByValue<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
    }
    public void Deactivate()
    {
        isActive = false;
        
        path.Clear();
        availableTiles.Clear();
        
        Events.BreakValueRelay<WalkableTile>(InputEvent.OnTileSelected, OnTileSelected);
        Events.BreakValueRelay<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    void OnTileSelected(WalkableTile selectedTile)
    {
        if (isActive && availableTiles.Contains(selectedTile))
        {
            foreach (var tile in nav.Map.Tiles.Values)
            {
                if (tile is WalkableTile walkableTile) walkableTile.SetMark(Mark.None);
            }
            
            nav.Move(path.ToArray());
            path.Clear();
            
            isActive = false;
        }
        else if (!nav.Target.IsMoving && selectedTile == nav.Current)
        {
            availableTiles = selectedTile.GetCellsAround(range).ToHashSet();
            availableTiles.Mark(Mark.Inactive);
            
            path.Add(nav.Current);
            isActive = true;
        }
    }

    void OnMouseMove(Vector2 mousePosition)
    {
        if (nav.Target.IsMoving) return;
        var cell = Inputs.GetCellAt(mousePosition);
        
        if (!isActive)
        {
            if (cell == nav.Current.Position) nav.Current.SetMark(Mark.Active);
            else nav.Current.SetMark(Mark.None);
        }
        else if (Current.Position != cell && nav.Current.Position != cell && cell.xy().TryGetTile(out var tile) && tile.IsFree())
        {
            var subIndex = -1;
            for (var i = 0; i < path.Count - 1; i++)
            {
                if (!path[i].IsNeighbourOf(tile)) continue;

                subIndex = i + 1;
                break;
            }

            if (subIndex == -1)
            {
                if (tile.IsNeighbourOf(Current))
                {
                    if (path.Count <= range)
                    {
                        path.Add(tile);
                        RefreshDisplay();
                    }
                    else TryReconstructPath(0, tile);
                }
                else 
                {
                    if (!TryReconstructPath(0, tile))
                    {
                        if (TryReconstructPath(path.Count - 1, tile)) RefreshDisplay();
                    }
                    else RefreshDisplay();
                }

                return;
            }
            
            var bracket = path.Count - subIndex;
            path.RemoveRange(subIndex, bracket);

            if (path.Count <= range) path.Add(tile);
            RefreshDisplay();
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    private bool TryReconstructPath(int startIndex, WalkableTile end)
    {
        var start = path[startIndex];

        if (start.x == end.x)
        {
            Func<Tile, int> axisPicker = item => item.y;
            Func<int, Vector2Int> cellMaker = value => new Vector2Int(start.x, value);
            
            path.RemoveRange(startIndex + 1, path.Count - startIndex - 1);
            path.AddRange((IEnumerable<WalkableTile>) ReconstructPath(start, end, range - (path.Count - 1), axisPicker, cellMaker));

            return true;
        }
        else if (start.y == end.y)
        {
            Func<Tile, int> axisPicker = item => item.x;
            Func<int, Vector2Int> cellMaker = value => new Vector2Int(value, start.y);

            path.RemoveRange(startIndex + 1, path.Count - startIndex - 1);
            path.AddRange((IEnumerable<WalkableTile>) ReconstructPath(start, end, range - (path.Count - 1), axisPicker, cellMaker));

            return true;
        }

        else return false;
    }
    private IEnumerable<WalkableTile> ReconstructPath(WalkableTile start, WalkableTile end, int range, Func<Tile, int> axisPicker, Func<int, Vector2Int> cellMaker)
    {
        var l = axisPicker(start);
        var r = axisPicker(end);

        var length = Mathf.Abs(l - r);
        if (length > range) length = range;

        var output = new List<WalkableTile>();
        if (l > r)
        {
            for (var i = 1; i < length; i++)
            {
                var value = l - i;
                if (!TryAddTile(value)) break;
            }
        }
        else
        {
            for (var i = 1; i < length; i++)
            {
                var value = l + i;
                if (!TryAddTile(value)) break;
            }
        }

        bool TryAddTile(int value)
        {
            var cell = cellMaker(value);
                
            if (!cell.TryGetTile(out var tile) || !tile.IsFree()) return false;
                            
            output.Add(tile);
            return true;
        }

        return output;
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    private void RefreshDisplay()
    {
        availableTiles.Mark(Mark.Inactive);
        foreach (var tile in path) tile.SetMark(Mark.Active);
    }
}
