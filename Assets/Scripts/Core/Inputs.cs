using Flux;
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

    private bool isActive;
    
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

        Events.RelayByVoid(GameEvent.OnTurnStart, Activate);
        Events.RelayByVoid(GameEvent.OnTurnEnd, Deactivate);
        Events.RelayByVoid(GameEvent.OnTileChange, OnTileChange);
        
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
        
        Events.BreakVoidRelay(GameEvent.OnTurnStart, Activate);
        Events.BreakVoidRelay(GameEvent.OnTurnEnd, Deactivate);
        Events.BreakVoidRelay(GameEvent.OnTileChange, OnTileChange);
        
        value.Disable();
    }

    void Activate() => isActive = true;
    void Deactivate() => isActive = false;
    
    void Update()
    {
        if (!isActive || Buffer.hasStopped) return;
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
    
    public void OnTileChange()
    {
        if (!isActive) return;
        
        if (hasHoveredTile) Events.ZipCall(InputEvent.OnTileHover, hoveredTile);
        else Events.ZipCall(InputEvent.OnTileHover, false);
    }

    void OnClick(InputAction.CallbackContext context)
    {
        if (!isActive) return;
        
        var cell = GetCellAt(mousePosition).xy();
        if (cell.TryGetTile(out var tile)) Events.ZipCall(InputEvent.OnTileSelected, tile);
    }
    void OnMouseMove(InputAction.CallbackContext context) => mousePosition = context.ReadValue<Vector2>();
}