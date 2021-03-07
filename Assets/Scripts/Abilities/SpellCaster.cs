using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;

public class Spellcaster : MonoBehaviour, ILink
{
    public static IReadOnlyDictionary<Id, CastArgs> EmptyArgs { get; } = new Dictionary<Id, CastArgs>();
    
    public event Action<ILink> onDestroyed; 
    
    public ITurnbound Owner { get; set; }
    
    [SerializeField] private Navigator nav;

    private bool isActive;

    private bool isStatic;
    private SpellBase current;
    private HashSet<Tile> availableTiles;
    
    private bool hasCaster;
    private IAttributeHolder caster;
    private IReadOnlyDictionary<Id, CastArgs> castArgs => hasCaster ? caster.Args : EmptyArgs;

    //------------------------------------------------------------------------------------------------------------------/

    void Awake() => hasCaster = TryGetComponent<IAttributeHolder>(out caster);
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public void Activate() => Events.RelayByValue<SpellBase,bool>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
    public void Deactivate()
    {
        Events.BreakValueRelay<SpellBase,bool>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
        if (!isActive) return;

        End();
    }

    private void Shutdown()
    {
        Extensions.ClearMarks();
        
        Events.BreakValueRelay<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
        Events.BreakValueRelay<Tile>(InputEvent.OnTileSelected, OnTileSelected);
    }
    private void End()
    {
        isActive = false;
        Inputs.isLocked = false;

        Shutdown();
    }

    //------------------------------------------------------------------------------------------------------------------/

    void OnDestroy()
    {
        Deactivate();
        onDestroyed?.Invoke(this);
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    void OnSpellSelected(SpellBase spell, bool isStatic)
    {
        if (Inputs.isLocked && !isActive) return;
        
        this.isStatic = isStatic;
        Inputs.isLocked = true;

        if (!isActive)
        {
            isActive = true;
            
            Events.RelayByValue<Tile>(InputEvent.OnTileSelected, OnTileSelected);
            Events.RelayByValue<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
        }
        Extensions.ClearMarks();
        
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
        if (!availableTiles.Contains(tile))
        {
            End();
            return;
        }
        
        Owner.IncreaseBusiness();
        current.onCastDone += OnCastDone;
        current.CastFrom(tile, castArgs);

        if (current.IsDone)
        {
            Events.ZipCall(GameEvent.OnSpellUsed, current, isStatic);
            Shutdown();
        }
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
        if (this == null) return; // TO DEBUG
        
        Owner.DecreaseBusiness();

        if (current.IsDone)
        {
            isActive = false;
            Routines.Start(Routines.DoAfter(() => Inputs.isLocked = false, new YieldFrame()));
        }
    }
}
