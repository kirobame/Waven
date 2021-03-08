using System;
using Flux;
using Flux.Event;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    void Awake() => Events.Register(InputEvent.OnTileHover, OnTileHover);
    void OnDestroy() => Events.Unregister(InputEvent.OnTileHover, OnTileHover);

    [SerializeField] private new SpriteRenderer renderer;
    
    void OnTileHover(EventArgs args)
    {
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