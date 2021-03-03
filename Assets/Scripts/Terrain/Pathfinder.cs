using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Flux.Data;

public class Pathfinder : MonoBehaviour, IActivable
{
    [SerializeField] private Navigator nav;
    [SerializeField, Range(0,5)] private int range;

    private Tile Current => path[path.Count - 1];
    private List<Tile> path = new List<Tile>();

    private InputAction clickAction;
    private InputAction mousePosAction;
    private bool isActive;

    private Vector2 mousePosition;
    
    //------------------------------------------------------------------------------------------------------------------/

    void Start()
    {
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        clickAction = inputs["Core/Click"];
        mousePosAction = inputs["Core/MousePosition"];
    }

    public void Activate()
    {
        clickAction.performed += OnClick;
        mousePosAction.performed += OnMouseMove;
    }
    public void Deactivate()
    {
        clickAction.performed -= OnClick;
        mousePosAction.performed -= OnMouseMove;
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    private Vector3Int GetCellUnderMouse()
    {
        var camera = Repository.Get<Camera>(References.Camera);
        var position = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -camera.transform.position.z));

        position.y -= nav.YOffset * 0.5f;
        position.z = 0.0f;
        
        return nav.Map.Tilemap.WorldToCell(position);
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    void OnClick(InputAction.CallbackContext context)
    {
        if (isActive)
        {
            foreach (var tile in nav.Map.Tiles.Values) tile.SetMark(Mark.None);
            
            nav.Move(path.ToArray());
            path.Clear();
            
            isActive = false;
        }
        else if (!nav.Target.IsMoving)
        {
            var cell = GetCellUnderMouse();
            if (cell != nav.Current.Position) return;
            
            nav.Map.MarkRange(nav.Current, range, Mark.Inactive);
            
            path.Add(nav.Current);
            isActive = true;
        }
    }

    void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
        
        if (nav.Target.IsMoving) return;
        var cell = GetCellUnderMouse();
        
        if (!isActive)
        {
            if (cell == nav.Current.Position) nav.Current.SetMark(Mark.Active);
            else nav.Current.SetMark(Mark.None);
        }
        else if (Current.Position != cell && nav.Current.Position != cell && nav.Map.Tiles.TryGetValue(cell.xy(), out var tile))
        {
            var subIndex = -1;
            for (var i = 0; i < path.Count - 1; i++)
            {
                if (!path[i].IsNeighbourOf(tile)) continue;

                subIndex = i + 1;
                break;
            }

            if (subIndex == -1)
            {
                if (tile.IsNeighbourOf(Current))
                {
                    if (path.Count <= range)
                    {
                        path.Add(tile);
                        RefreshDisplay();
                    }
                    else TryReconstructPath(0, tile);
                }
                else 
                {
                    if (!TryReconstructPath(0, tile))
                    {
                        if (TryReconstructPath(path.Count - 1, tile)) RefreshDisplay();
                    }
                    else RefreshDisplay();
                }

                return;
            }
            
            var bracket = path.Count - subIndex;
            path.RemoveRange(subIndex, bracket);

            if (path.Count <= range) path.Add(tile);
            RefreshDisplay();
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    private bool TryReconstructPath(int startIndex, Tile end)
    {
        var start = path[startIndex];

        if (start.x == end.x)
        {
            Func<Tile, int> axisPicker = item => item.y;
            Func<int, Vector2Int> cellMaker = value => new Vector2Int(start.x, value);
            
            path.RemoveRange(startIndex + 1, path.Count - startIndex - 1);
            path.AddRange(ReconstructPath(start, end, range - (path.Count - 1), axisPicker, cellMaker));

            return true;
        }
        else if (start.y == end.y)
        {
            Func<Tile, int> axisPicker = item => item.x;
            Func<int, Vector2Int> cellMaker = value => new Vector2Int(value, start.y);

            path.RemoveRange(startIndex + 1, path.Count - startIndex - 1);
            path.AddRange(ReconstructPath(start, end, range - (path.Count - 1), axisPicker, cellMaker));

            return true;
        }

        else return false;
    }
    private IEnumerable<Tile> ReconstructPath(Tile start, Tile end, int range, Func<Tile, int> axisPicker, Func<int, Vector2Int> cellMaker)
    {
        var l = axisPicker(start);
        var r = axisPicker(end);

        var length = Mathf.Abs(l - r);
        if (length > range) length = range;

        var output = new List<Tile>();
        if (l > r)
        {
            for (var i = 1; i < length; i++)
            {
                var value = l - i;
                if (!TryAddTile(value)) break;
            }
        }
        else
        {
            for (var i = 1; i < length; i++)
            {
                var value = l + i;
                if (!TryAddTile(value)) break;
            }
        }

        bool TryAddTile(int value)
        {
            var cell = cellMaker(value);
                
            if (!nav.Map.Tiles.TryGetValue(cell, out var tile)) return false;
                            
            output.Add(tile);
            return true;
        }

        return output;
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    private void RefreshDisplay()
    {
        nav.Map.MarkRange(nav.Current, range, Mark.Inactive);
        foreach (var tile in path) tile.SetMark(Mark.Active);
    }
}
