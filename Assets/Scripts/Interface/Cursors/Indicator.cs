using System;
using Flux;
using Flux.Event;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    void Awake()
    {
        Events.RelayByVoid(GameEvent.OnTurnStart, Activate);
        Events.RelayByVoid(GameEvent.OnTurnEnd, Deactivate);
        Events.Register(InputEvent.OnTileHover, OnTileHover);
    }
    void OnDestroy()
    {
        Events.BreakVoidRelay(GameEvent.OnTurnStart, Activate);
        Events.BreakVoidRelay(GameEvent.OnTurnEnd, Deactivate);
        Events.Unregister(InputEvent.OnTileHover, OnTileHover);
    }

    [SerializeField] private new SpriteRenderer renderer;

    private bool isActive;

    void Activate()
    {
        isActive = true;
        renderer.enabled = true;
    }
    void Deactivate()
    {
        isActive = false;
        renderer.enabled = false;
    }
    
    void OnTileHover(EventArgs args)
    {
        if (!isActive) return;

        if (args is IWrapper<Tile> wrapper)
        {
            foreach (var entity in wrapper.Value.Entities)
            {
                var cursor = ((Component)entity).GetComponentInChildren<Cursor>();
                if (cursor == null || !cursor.IsActive) continue;

                renderer.enabled = false;
                return;
            }

            transform.position = wrapper.Value.GetWorldPosition();
            renderer.enabled = true;
        }
        else renderer.enabled = false;
    }
}