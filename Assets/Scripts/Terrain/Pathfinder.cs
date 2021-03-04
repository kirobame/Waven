using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Flux;
using Flux.Data;
using Flux.Event;
using Sirenix.Utilities;

public class Pathfinder : MonoBehaviour, ILink
{
    public event Action<ILink> onDestroyed; 
    
    public ITileable Owner { get; set; }
    
    [SerializeField] private Navigator nav;
    [SerializeField, Range(0,5)] private int range;

    private Tile Current => path[path.Count - 1];
    private List<Tile> path = new List<Tile>();

    private HashSet<Tile> availableTiles = new HashSet<Tile>();
    private bool isActive;
 
    //------------------------------------------------------------------------------------------------------------------/
    
    public void Activate()
    {
        Events.RelayByValue<Tile>(InputEvent.OnTileSelected, OnTileSelected);
        Events.RelayByValue<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
    }
    public void Deactivate()
    {
        nav.Current.Mark(Mark.None);
        if (isActive) Shutdown();
        
        path.Clear();
        availableTiles.Clear();
        
        Events.BreakValueRelay<Tile>(InputEvent.OnTileSelected, OnTileSelected);
        Events.BreakValueRelay<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    void OnDestroy() => onDestroyed?.Invoke(this);
    
    //------------------------------------------------------------------------------------------------------------------/

    void OnTileSelected(Tile selectedTile)
    {
        if (Inputs.isLocked && !isActive) return;
        
        if (isActive && availableTiles.Contains(selectedTile))
        {
            if (path.Count > 1) nav.Move(path.ToArray());
            Shutdown();
        }
        else if (!nav.Target.IsMoving && selectedTile == nav.Current)
        {
            Inputs.isLocked = true;
            
            availableTiles = selectedTile.GetCellsAround(range).ToHashSet();
            availableTiles.Mark(Mark.Inactive);
            
            path.Add(nav.Current);
            isActive = true;
        }
    }

    void OnMouseMove(Vector2 mousePosition)
    {
        if (Inputs.isLocked && !isActive) return;
        
        if (nav.Target.IsMoving) return;
        var cell = Inputs.GetCellAt(mousePosition);
        
        if (!isActive)
        {
            if (cell == nav.Current.Position) nav.Current.Mark(Mark.Active);
            else nav.Current.Mark(Mark.None);
        }
        else 
        {
            if (cell == nav.Current.Position)
            {
                path.Clear();
                path.Add(nav.Current);
                
                Refresh();
            }
            else if (TryGetTile(cell, out var tile))
            {
                for (var i = 0; i < path.Count; i++)
                {
                    if (!TryReconstructionBetween(i, tile)) continue;
                
                    Refresh();
                    break;
                }
            }
        }
    }

    private bool TryGetTile(Vector3Int cell, out Tile tile)
    {
        var similarity = Current.Position != cell;
        var validity = cell.xy().TryGetTile(out tile) && tile.IsFree();
        var availability = availableTiles.Contains(tile);

        return similarity && validity && availability;
    }
    
    //------------------------------------------------------------------------------------------------------------------/
    
    private bool TryReconstructionBetween(int startIndex, Tile end)
    {
        var start = path[startIndex];

        if (start.x == end.x) return Try(item => item.y, value => new Vector2Int(start.x, value));
        else if (start.y == end.y) return Try(item => item.x, value => new Vector2Int(value, start.y));
        else return false;

        bool Try(Func<TileBase, int> axisPicker, Func<int, Vector2Int> cellMaker)
        {
          
            var range = this.range - startIndex;
            var list = ReconstructPath(start, end, range, axisPicker, cellMaker);
            
            if (!list.Any()) return false;

            path.RemoveRange(startIndex + 1, path.Count - startIndex - 1);
            path.AddRange(list);

            return true;
        }
    }
    private IEnumerable<Tile> ReconstructPath(Tile start, Tile end, int range, Func<TileBase, int> axisPicker, Func<int, Vector2Int> cellMaker)
    {
        var l = axisPicker(start);
        var r = axisPicker(end);

        var length = Mathf.Abs(l - r) + 1;
        if (length > range + 1) length = range + 1;
        
        var output = new List<Tile>();
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

    private void Refresh()
    {
        availableTiles.Mark(Mark.Inactive);
        foreach (var tile in path) tile.Mark(Mark.Active);
    }
    private void Shutdown()
    { 
        foreach (var tile in availableTiles) { if (tile is Tile walkableTile) walkableTile.Mark(Mark.None); }
        path.Clear();
        
        isActive = false;
    }
}
