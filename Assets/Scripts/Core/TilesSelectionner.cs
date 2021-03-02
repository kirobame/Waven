using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Event;
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
        Repository.Get<InputAction>(References.Inputs).Enable();
        clickAction.performed += OnClick;
        mousePosAction.performed += OnMouseMove;

        Events.Open(SelectionEvents.OnTileSelected); //Move dans le start ?
    }

    private void OnDisable()
    {
        clickAction.performed -= OnClick;
        mousePosAction.performed -= OnMouseMove;

        //Clear events ?
    }

    private void Start()
    {
        map = Repository.Get<Map>(References.Map);
    }

    void OnClick(InputAction.CallbackContext context)
    {
        var mouseToTile = map.Tilemap.WorldToCell(mousePosition);
        Debug.Log(mouseToTile);
        Events.ZipCall<Vector3Int>(SelectionEvents.OnTileSelected, mouseToTile);
    }

    void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2Int>();
    }
}
