﻿using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    private static bool trueLock;

    public static bool isLocked
    {
        get => trueLock;
        set
        {
            Events.ZipCall(InputEvent.OnInputLock, value);
            trueLock = value;
        }
    }

    public static Vector3Int GetCellAt(Vector2 screenPosition)
    {
        var camera = Repository.Get<Camera>(References.Camera);
        var position = camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -camera.transform.position.z));

        position.y -= Navigator.YOffset * 0.5f;
        position.z = 0.0f;

        var map = Repository.Get<Map>(References.Map);
        return map.Tilemap.WorldToCell(position);
    }
    
    private InputAction clickAction;
    private InputAction mouseMoveAction;
    
    private InputActionAsset value;

    private bool hasHoveredTile;
    private Tile hoveredTile;
    
    private Vector2 mousePosition;

    void Start()
    {
        Events.Open(InputEvent.OnTileSelected);
        Events.Open(InputEvent.OnMouseMove);
        Events.Open(InputEvent.OnInterrupt);
        Events.Open(InputEvent.OnTileHover);
        Events.Open(InputEvent.OnInputLock);

        isLocked = false;
        
        Routines.Start(Routines.DoAfter(() =>
        {
            value = Repository.Get<InputActionAsset>(References.Inputs);
            value.Enable();
            
            clickAction = value["Core/Click"];
            clickAction.performed += OnClick;
            
            mouseMoveAction = value["Core/MousePosition"];
            mouseMoveAction.performed += OnMouseMove;

        }, new YieldFrame()));
    }
    void OnDestroy()
    {
        clickAction.performed -= OnClick;
        mouseMoveAction.performed -= OnMouseMove;
        
        value.Disable();
    }

    void Update()
    {
        Events.ZipCall(InputEvent.OnMouseMove, mousePosition);
        
        var cell = GetCellAt(mousePosition).xy();
        if (!cell.TryGetTile(out var output) || output is DeathTile)
        {
            if (!hasHoveredTile) return;

            hasHoveredTile = false;
            Events.ZipCall(InputEvent.OnTileHover, false);
        }
        else
        {
            if (output == hoveredTile) return;

            hasHoveredTile = true;
            Events.ZipCall(InputEvent.OnTileHover, output);
        }

        hoveredTile = output;
    }

    void OnClick(InputAction.CallbackContext context)
    {
        var cell = GetCellAt(mousePosition).xy();
        if (cell.TryGetTile(out var tile)) Events.ZipCall(InputEvent.OnTileSelected, tile);
    }
    void OnMouseMove(InputAction.CallbackContext context) => mousePosition = context.ReadValue<Vector2>();
}