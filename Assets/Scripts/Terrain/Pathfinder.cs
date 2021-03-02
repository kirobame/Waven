using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Flux.Data;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] private Navigator nav;

    private Tile Current => path[path.Count - 1];
    private List<Tile> path = new List<Tile>();

    private InputAction clickAction;
    private InputAction mousePosAction;
    private bool isActive;

    private Vector2 mousePosition;

    void Start()
    {
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        
        clickAction = inputs["Core/Click"];
        clickAction.performed += OnClick;
        
        mousePosAction = inputs["Core/MousePosition"];
        mousePosAction.performed += OnMouseMove;
    }
    void OnDestroy()
    {
        clickAction.performed -= OnClick;
        mousePosAction.performed -= OnMouseMove;
    }

    void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("CLICK");
        
        if (isActive)
        {
            isActive = false;
        }
        else
        {
            var cell = GetCellUnderMouse();
            Debug.Log($"Gotten : {cell} // Current : {nav.Current.Position}");
            
            if (cell != nav.Current.Position) return;
            
            Debug.Log("BEGINNING PATH CREATION");
            
            path.Add(nav.Current);
            isActive = true;
        }
    }

    void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        
        if (!isActive) return;

        var cell = GetCellUnderMouse();
        if (Current.Position == cell || !nav.Map.Tiles.TryGetValue(cell.xy(), out var tile)) return;

        var subIndex = -1;
        for (var i = 0; i < path.Count - 1; i++)
        {
            if (!path[i].IsNeighbourOf(tile)) continue;

            subIndex = i + 1;
            break;
        }

        if (subIndex == -1)
        {
            path.Add(tile);
            return;
        }

        var range = path.Count - subIndex;
        path.RemoveRange(subIndex, range);
        
        path.Add(tile);
    }

    private Vector3Int GetCellUnderMouse()
    {
        var camera = Repository.Get<Camera>(References.Camera);
        var position = camera.ScreenToWorldPoint(mousePosition);
        position.z = 0.0f;

        return nav.Map.Tilemap.WorldToCell(position);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || !isActive) return;

        foreach (var tile in path)
        {
            var position = nav.Map.Tilemap.CellToWorld(tile.Position);
            Gizmos.DrawSphere(position, 0.25f);
        }
    }
}
