using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;

public class Spellcaster : MonoBehaviour, ILink
{
    public static Spellcaster Active { get; private set; }
    public static IReadOnlyDictionary<Id, List<CastArgs>> EmptyArgs { get; } = new Dictionary<Id, List<CastArgs>>();
    
    public event Action<ILink> onDestroyed; 
    
    public ITurnbound Owner { get; set; }

    public SpellBase Current => current;
    public IEnumerable<Tile> AvailableTiles => availableTiles;
    
    [SerializeField] private Navigator nav;

    private bool isActive;
    private bool isWaiting;

    private bool isStatic;
    private SpellBase current;
    private HashSet<Tile> availableTiles;
    
    private bool hasCaster;
    private IAttributeHolder caster;
    private IReadOnlyDictionary<Id, List<CastArgs>> castArgs => hasCaster ? caster.Args : EmptyArgs;

    //------------------------------------------------------------------------------------------------------------------/

    void Awake() => hasCaster = TryGetComponent<IAttributeHolder>(out caster);
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public void Activate() => Events.RelayByValue<SpellBase,bool>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
    public void Deactivate()
    {
        Events.BreakValueRelay<SpellBase,bool>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
        
        if (!isActive) return;
        
        current = null;
        End();
    }

    private void Shutdown()
    {
        Extensions.ClearMarks();
        
        Events.EmptyCall(InterfaceEvent.OnSpellEnd);
        
        Events.Unregister(InputEvent.OnInterrupt, OnInterrupt);
        Events.BreakValueRelay<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
        Events.BreakValueRelay<Tile>(InputEvent.OnTileSelected, OnTileSelected);
    }
    private void End()
    {
        Active = null;

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

    void OnInterrupt(EventArgs args)
    {
        if (args is IWrapper<bool> wrapper)
        {
            if (wrapper.Value)
            {
                if (!isActive || isWaiting) return;
                End();
            }
            else
            {
                
              
                if (!isActive || isWaiting || current.CastingPatterns.Any(pattern => pattern is Point)) return;
                End();
            }
        }
        else if (args is IWrapper<Tile> tileWrapper)
        {
            if (!isActive || isWaiting || current.GetTilesForCasting(tileWrapper.Value, castArgs).HasIntersection()) return;
            End();
        }
        else
        {
            if (!isActive || isWaiting || current.CastingPatterns.Any(pattern => pattern is Point)) return;
            End();
        }
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    void OnSpellSelected(SpellBase spell, bool isStatic)
    {
        if (!isActive && Inputs.isLocked)
        {
            Events.EmptyCall(InputEvent.OnInterrupt);
            if (Inputs.isLocked) return;
        }
        
        this.isStatic = isStatic;

        Inputs.isLocked = true;
        Extensions.ClearMarks();

        Active = this;
        current = spell;
        Setup();
        
        if (!isActive)
        {
            Events.ZipCall(InterfaceEvent.OnSpellTilesAffect, this);
            
            isActive = true;
            isWaiting = false;
            
            Events.Register(InputEvent.OnInterrupt, OnInterrupt);
            Events.RelayByValue<Tile>(InputEvent.OnTileSelected, OnTileSelected);
            Events.RelayByValue<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
        }
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
        if (!isActive) return;
        
        if (!availableTiles.Contains(tile))
        {
            End();
            return;
        }

        Buffer.consumeTriggerSpell = true;
        Owner.IncreaseBusiness();
        isWaiting = true;
        
        current.onCastDone += OnCastDone;
        
        if (Player.Active.Navigator.Current != tile) Player.Active.SetOrientation((tile.GetWorldPosition() - Player.Active.transform.position).xy().ComputeOrientation());
        current.CastFrom(tile, castArgs);

        if (current.IsDone)
        {
            Events.ZipCall(GameEvent.OnSpellUsed, current, isStatic);
            Events.ZipCall(ChallengeEvent.OnSpellUse, 1);
            
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
        isWaiting = false;
        current.onCastDone -= OnCastDone;
        
        if (this == null) return; // TO DEBUG

        Owner.DecreaseBusiness();
        if (current.IsDone)
        {
            isActive = false;
            Routines.Start(Routines.DoAfter(() =>
            {
                current = null;
                Inputs.isLocked = false;
                
            }, new YieldFrame()));
        }
    }
}
