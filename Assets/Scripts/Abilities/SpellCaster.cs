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
    
    public ITurnbound Owner { get; set; }
    
    [SerializeField] private Navigator nav;

    private bool isActive;
    
    private SpellBase current;
    private HashSet<Tile> availableTiles;
    
    private bool hasCaster;
    private ITempCaster caster;
    private IReadOnlyDictionary<Id, CastArgs> castArgs => hasCaster ? caster.Args : new Dictionary<Id, CastArgs>();

    //------------------------------------------------------------------------------------------------------------------/

    void Awake() => hasCaster = TryGetComponent<ITempCaster>(out caster);
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public void Activate() => Events.RelayByValue<SpellBase>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
    public void Deactivate()
    {
        if (!isActive) return;
        
        isActive = false;
        Inputs.isLocked = false;

        Shutdown();
        Events.BreakValueRelay<SpellBase>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
    }

    private void Shutdown()
    {
        Extensions.ClearMarks();
        
        Events.BreakValueRelay<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
        Events.BreakValueRelay<Tile>(InputEvent.OnTileSelected, OnTileSelected);
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    void OnDestroy()
    {
        Deactivate();
        onDestroyed?.Invoke(this);
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    void OnSpellSelected(SpellBase spell)
    {
        if (Inputs.isLocked) return;

        isActive = true;
        Inputs.isLocked = true;
        
        Events.RelayByValue<Tile>(InputEvent.OnTileSelected, OnTileSelected);
        Events.RelayByValue<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
        
        current = spell;
        Setup();
    }

    private void Setup()
    {
        current.Prepare();

        availableTiles = current.GetTilesForCasting(nav.Current, castArgs);
        availableTiles.Mark(Mark.Inactive);
    }
    
    //------------------------------------------------------------------------------------------------------------------/
    
    void OnTileSelected(Tile tile)
    {
        if (!availableTiles.Contains(tile)) return;

        Events.ZipCall<SpellBase, HashSet<Tile>>(InterfaceEvent.OnSpellCast, current, availableTiles);
        Owner.IncreaseBusiness();
        current.onCastDone += OnCastDone;
        current.CastFrom(tile, castArgs);
        
        if (current.IsDone) Shutdown();
        else Setup();
    }
    void OnMouseMove(Vector2 mousePosition)
    {
        Extensions.ClearMarks();
        availableTiles.Mark(Mark.Inactive);
        
        var cell = Inputs.GetCellAt(mousePosition);
        if (!cell.xy().TryGetTile(out var tile) || !availableTiles.Contains(tile)) return;

        
        var affectedTiles = current.GetAffectedTilesFor(tile, castArgs);
        affectedTiles.Mark(Mark.Active);
    }

    void OnCastDone()
    {
        current.onCastDone -= OnCastDone;
        Owner.DecreaseBusiness();

        if (current.IsDone)
        {
            isActive = false;
            Routines.Start(Routines.DoAfter(() => Inputs.isLocked = false, new YieldFrame()));
        }
    }
}
