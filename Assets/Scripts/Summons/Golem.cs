using System;
using System.Collections;
using System.Collections.Generic;
using Flux.Event;
using UnityEngine;

public class Golem : ExtendedTileable, ILink
{
    public event Action<ILink> onDestroyed;

    public ITurnbound Owner
    {
        get => player;
        set => player = (Player)value;
    }
    private Player player;

    public bool HasAura
    {
        get => hasAura;
        set
        {
            if (value && !hasAura)
            {
                Events.RelayByValue<ITileable>(GameEvent.OnTileChange, OnTileChange);
                ActivateAuraFully();
            }
            else if (!value && hasAura)
            {
                Events.BreakValueRelay<ITileable>(GameEvent.OnTileChange, OnTileChange);
            }
            hasAura = value;
        }
    }
    private bool hasAura;

    public int activation;
    [Space, SerializeField] private SpellBase aura;

    private bool hasOwner;

    protected override void OnDestroy()
    {
        base.OnDestroy();

        hasAura = false;
        if (!hasOwner) return;
        
        onDestroyed?.Invoke(this);
    }
    
    public void LinkTo(Player player)
    {
        if (hasOwner)
        {
            this.player.RemoveDependency(gameObject);
            Team = TeamTag.Neutral;
        }
        
        hasOwner = true;
        player.AddDependency(gameObject);
    }

    public void Activate() { }
    public void Deactivate()
    {
        activation--;
        if (activation <= 0)
        {
            activation = 0;
            if (hasOwner)
            {
                player.RemoveDependency(gameObject);
                Team = TeamTag.Neutral;
                
                hasOwner = false;
            }
        }
    }
    
    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false, bool processDir = true)
    {
        if (hasOwner) Owner.IncreaseBusiness();
        base.Move(path, speed, overrideSpeed, processDir);
    }
    protected override void ProcessNewTile(Tile tile)
    {
        if (!hasAura) return;
        ActivateAuraFully();
    }

    protected override void OnMoveCompleted()
    {
        base.OnMoveCompleted();
        
        if (!hasOwner) return;
        Owner.DecreaseBusiness();
    }

    void OnTileChange(ITileable tileable)
    {
        var tile = tileable.Navigator.Current;
        var currentTile = navigator.Current;
        
        if (tile.x == currentTile.x && Mathf.Abs(tile.y - currentTile.y) == 1) ActivateAura(tile);
        else if (tile.y == currentTile.y && Mathf.Abs(tile.x - currentTile.x) == 1) ActivateAura(tile);
    }

    private void ActivateAuraFully()
    {
        var directions = new Vector2Int[]
        {
            Vector2Int.down,
            Vector2Int.left, 
            Vector2Int.right, 
            Vector2Int.up
        };
        foreach (var direction in directions)
        {
            var cell = Navigator.Current.FlatPosition + direction;
            if (cell.TryGetTile(out var checkedTile)) ActivateAura(checkedTile);
        }
    }
    private void ActivateAura(Tile tile)
    {
        aura.Prepare();
        aura.CastFrom(tile, Spellcaster.EmptyArgs);
    }
}
