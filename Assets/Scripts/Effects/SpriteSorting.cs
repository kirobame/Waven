using System;
using Flux;
using Flux.Event;
using UnityEngine;

[Serializable, Path("General")]
public class SpriteSorting : Flux.Feedbacks.Effect
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private string layer;
    [SerializeField] private int order;
    
    protected override void OnUpdate(EventArgs args)
    {
        if (args is FallArgs wrapper)
        {
            renderer.sortingLayerName = wrapper.Layer;
            renderer.sortingOrder = wrapper.Order;
        }
        else
        {
            renderer.sortingLayerName = layer;
            renderer.sortingOrder = order;
        }
        
        IsDone = true;
    }
}