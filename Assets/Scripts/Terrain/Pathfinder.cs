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
    
    public ITurnbound Owner { get; set; }
    
    [SerializeField] private Moveable nav;
    
    [Space, SerializeField] private Spell attack;
    [SerializeField] private int attackCount;

    private Tile Current => path[path.Count - 1];
    private List<Tile> path = new List<Tile>();

    private HashSet<Tile> availableTiles = new HashSet<Tile>();
    private bool isActive;
    private bool isWaiting;

    private bool shouldAttack;
    private int attackCounter;
    private Tile tileToAttack;
    
    private bool hasCaster;
    private IAttributeHolder caster;
    private IReadOnlyDictionary<Id, CastArgs> castArgs => hasCaster ? caster.Args : Spellcaster.EmptyArgs;

    //------------------------------------------------------------------------------------------------------------------/
    
    public void Activate()
    {
        attackCounter = attackCount;
        
        Events.RelayByValue<Tile>(InputEvent.OnTileSelected, OnTileSelected);
        Events.RelayByValue<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
    }
    public void Deactivate()
    {
        nav.Current.Mark(Mark.None);
        if (isActive)
        {
            Inputs.isLocked = false;
            Shutdown();
        }
        
        path.Clear();
        availableTiles.Clear();
        
        Events.BreakValueRelay<Tile>(InputEvent.OnTileSelected, OnTileSelected);
        Events.BreakValueRelay<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    void Awake() => hasCaster = TryGetComponent<IAttributeHolder>(out caster);
    void OnDestroy()
    {
        Deactivate();
        onDestroyed?.Invoke(this);
    }

    void OnInterrupt()
    {
        if (isWaiting) return;
        
        Inputs.isLocked = false;
        Shutdown();
    }

    //------------------------------------------------------------------------------------------------------------------/

    void OnTileSelected(Tile selectedTile)
    {
        if (nav.PM <= 0) return;

        if (isActive && Inputs.isLocked)
        {
            if (selectedTile == nav.Current || !availableTiles.Contains(selectedTile))
            {
                Inputs.isLocked = false;
                Shutdown();
                
                return;
            }

            if (shouldAttack)
            {
                attackCounter--;
                
                var lastIndex = path.Count - 1;
                tileToAttack = path[lastIndex];
                
                path.RemoveAt(lastIndex);
                    
                if (path.Count == 1)
                {
                    Attack();
                    shouldAttack = false;
                }
            }
            
            if (path.Count > 1)
            {
                nav.PM -= path.Count - 1;

                isWaiting = true;
                nav.Target.onMoveDone += OnMoveDone;
                nav.Move(path.ToArray());
            }
            else Inputs.isLocked = false;
            
            Shutdown();
        }
        else if (!nav.Target.IsMoving && selectedTile == nav.Current)
        {
            if (Inputs.isLocked)
            {
                Events.EmptyCall(InputEvent.OnInterrupt);
                if (Inputs.isLocked) return;
            }
            
            Inputs.isLocked = true;
            Events.RelayByVoid(InputEvent.OnInterrupt, OnInterrupt);

            isWaiting = false;
            shouldAttack = false;

            availableTiles = selectedTile.GetCellsAround(nav.PM).ToHashSet();
            availableTiles.Mark(Mark.Inactive);
            
            path.Add(nav.Current);
            isActive = true;
        }
    }

    void OnMouseMove(Vector2 mousePosition)
    {
        if (Inputs.isLocked && !isActive || nav.PM <= 0) return;
        
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
                    if (i != 0 && !path[i].IsFree() || !TryReconstructionBetween(i, tile)) continue;
                
                    Refresh();
                    break;
                }
            }
        }
    }

    private bool TryGetTile(Vector3Int cell, out Tile tile)
    {
        var similarity = Current.Position != cell;
        var validity = cell.xy().TryGetTile(out tile);
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
          
            var range = this.nav.PM - startIndex;
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
            if (!cell.TryGetTile(out var tile) || tile is DeathTile) return false;

            if (attackCounter > 0)
            {
                shouldAttack = false;
                foreach (var entity in tile.Entities)
                {
                    if (!entity.TryGet<IDamageable>(Player.Active.Team, out var damageable)) continue;

                    shouldAttack = true;
                    output.Add(tile);
                
                    return false;
                }
            }

            if (tile.IsFree())
            {
                output.Add(tile);
                return true;
            }
            else return false;
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
        Events.BreakVoidRelay(InputEvent.OnInterrupt, OnInterrupt);
        
        foreach (var tile in availableTiles) { if (tile is Tile walkableTile) walkableTile.Mark(Mark.None); }
        path.Clear();
        
        isActive = false;
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    private void Attack()
    {
        Player.Active.SetOrientation((tileToAttack.GetWorldPosition() - Player.Active.transform.position).xy().ComputeOrientation());
        
        attack.Prepare();
        attack.CastFrom(tileToAttack, castArgs);
    }
    
    void OnMoveDone(ITileable tileable)
    {
        isWaiting = false;

        if (shouldAttack) Attack();
        
        nav.Target.onMoveDone -= OnMoveDone;
        Inputs.isLocked = false;
    }
}
