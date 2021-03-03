using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
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

    private Vector2 mousePosition;
    
    void Start()
    {
        Events.Open(InputEvent.OnTileSelected);
        Events.Open(InputEvent.OnMouseMove);
        
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

    void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("OnClick");
        var cell = GetCellAt(mousePosition).xy();
        if (cell.TryGetTile(out var tile))
        {
            Debug.Log("TryGetTile");
            Events.ZipCall(InputEvent.OnTileSelected, tile);
        }
    }
    void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        Events.ZipCall(InputEvent.OnMouseMove, mousePosition);
    }
}