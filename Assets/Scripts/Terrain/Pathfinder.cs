using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Flux.Data;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] private Navigator nav;
    private Map map;

    public IReadOnlyDictionary<Vector2Int, Tile> Path => path;
    private Dictionary<Vector2Int, Tile> path = new Dictionary<Vector2Int, Tile>();

    private InputAction clickAction;
    private InputAction mousePosAction;
    private bool isCreatingPath;

    private void OnEnable()
    {
        clickAction.performed += OnClick;
        mousePosAction.performed += OnMouseMove;
    }

    private void OnDisable()
    {
        clickAction.performed -= OnClick;
        mousePosAction.performed -= OnMouseMove;
    }

    private void Start()
    {
        map = Repository.Get<Map>(References.Map);
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        clickAction = inputs["Core/Click"];
        mousePosAction = inputs["Core/MousePosition"];
    }

    void OnClick(InputAction.CallbackContext context)
    {
        if (isCreatingPath)
        {

        }
        else
        {
            
        }
        var startPos = nav.target.lastPosition;
        var flattenPostion = new Vector2Int((int)startPos.x, (int)startPos.y);
        CreatePath(flattenPostion);
    }

    void OnMouseMove(InputAction.CallbackContext context)
    {

    }

    private void CreatePath(Vector2Int startPos)
    {
        
        Tile defaultTile = default;
        path.Add(startPos, defaultTile);

        var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        var mouseToTile = map.Tilemap.WorldToCell(mousePos);
        var flattenMouseToTile = new Vector2Int(mouseToTile.x, mouseToTile.y);

        if (!nav.IsTileValid(flattenMouseToTile, out var tile)) return;
        //Add tile to dictionnary

        /*if (Path.TryGetValue(tile.Position, out))
        {

        }*/
    }
}
