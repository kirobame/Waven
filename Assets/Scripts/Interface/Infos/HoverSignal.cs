using System;
using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoverSignal : MonoBehaviour
{
    public static int activeId;
    
    [SerializeField] private Tileable root;

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
        if (activeId == 0 && previousOverlap) previousOverlap = false;
        
        var id = GetInstanceID();
        if (activeId != 0 && activeId != id) return;

        bool overlap;
        bool hasTile = false;
        Tile newTile = null;

        if (args is IWrapper<Tile> wrapper)
        {
            hasTile = true;
            newTile = wrapper.Value;
            
            overlap = wrapper.Value == navigator.Current;
        }
        else overlap = false;
        
        if (overlap == true && previousOverlap == false)
        {
            Events.ZipCall(InterfaceEvent.OnHoverStart, anchor, root);
            activeId = id;
            
            previousOverlap = true;
        }
        else if (overlap == false && previousOverlap == true)
        {
            Events.ZipCall(InterfaceEvent.OnHoverEnd, root);
            activeId = 0;
            
            previousOverlap = false;
            if (hasTile) Events.ZipCall(InputEvent.OnTileHover, newTile);
        }
       
    }
}