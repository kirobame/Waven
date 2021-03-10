using System;
using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoverSignal : MonoBehaviour
{
    public static int activeId;
    
    [SerializeField] private GameObject root;

    [Space, SerializeField] private InfoAnchor anchor;
    [SerializeField] private Navigator navigator;

    private bool previousOverlap;

    void Awake()
    {
        activeId = 0;
        Events.Register(InputEvent.OnTileHover, OnTileHover);
    }

    void OnDestroy()
    {
        Events.Unregister(InputEvent.OnTileHover, OnTileHover);
        
        if (!previousOverlap) return;
        
        Events.ZipCall(InterfaceEvent.OnHoverEnd, root);
        activeId = 0;
    }

    void OnTileHover(EventArgs args)
    {
        var id = GetInstanceID();
        if (activeId != 0 && activeId != id) return;

        bool overlap;
        if (args is IWrapper<Tile> wrapper) overlap = wrapper.Value == navigator.Current;
        else overlap = false;
        
        if (overlap == true && previousOverlap == false)
        {
            Events.ZipCall(InterfaceEvent.OnHoverStart, anchor, root);
            activeId = id;
        }
        else if (overlap == false && previousOverlap == true)
        {
            Events.ZipCall(InterfaceEvent.OnHoverEnd, root);
            activeId = 0;
        }
        previousOverlap = overlap;
    }
}