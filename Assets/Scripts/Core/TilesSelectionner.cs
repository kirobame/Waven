using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TilesSelectionner : MonoBehaviour
{
    private Map map;

    private InputAction clickAction;
    private InputAction mousePosAction;

    private Vector2 mousePosition;

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
    }

    void OnClick(InputAction.CallbackContext context)
    {
        var mouseToTile = map.Tilemap.WorldToCell(mousePosition);
        Debug.Log(mouseToTile);
    }

    void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2Int>();
    }
}
