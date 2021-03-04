using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;

public class Spellcaster : MonoBehaviour, ILink
{
    public event Action<ILink> onDestroyed; 
    
    public ITileable Owner { get; set; }
    
    private SpellBase current;
    private HashSet<Tile> availableTiles;
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public void Activate() => Events.RelayByValue<SpellBase>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
    public void Deactivate()
    {
        Shutdown();
        Events.BreakValueRelay<SpellBase>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
    }

    private void Shutdown()
    {
        Extensions.ClearMarks();
        
        Events.BreakValueRelay<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
        Events.BreakValueRelay<Tile>(InputEvent.OnTileSelected, OnTileSelected);

        Routines.Start(Routines.DoAfter(() => Inputs.isLocked = false, new YieldFrame()));
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    void OnDestroy() => onDestroyed?.Invoke(this);

    //------------------------------------------------------------------------------------------------------------------/
    
    void OnSpellSelected(SpellBase spell)
    {
        if (Inputs.isLocked) return;
        Inputs.isLocked = true;
        
        Events.RelayByValue<Tile>(InputEvent.OnTileSelected, OnTileSelected);
        Events.RelayByValue<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
        
        current = spell;
        Setup();
    }
    private void Setup()
    {
        current.Prepare();
        
        availableTiles = current.CastingPatterns.Accumulate(Owner.Navigator.Current);
        availableTiles.Mark(Mark.Inactive);
    }
    
    //------------------------------------------------------------------------------------------------------------------/
    
    void OnTileSelected(Tile tile)
    {
        if (!availableTiles.Contains(tile)) return;
        
        current.CastFrom(tile);

        if (current.IsDone) Shutdown();
        else Setup();
    }
    void OnMouseMove(Vector2 mousePosition)
    {
        Extensions.ClearMarks();
        availableTiles.Mark(Mark.Inactive);
        
        var cell = Inputs.GetCellAt(mousePosition);
        if (!cell.xy().TryGetTile(out var tile) || !availableTiles.Contains(tile)) return;

        var affectedTiles = current.GetAffectedTilesFor(tile);
        affectedTiles.Mark(Mark.Active);
    }
}
