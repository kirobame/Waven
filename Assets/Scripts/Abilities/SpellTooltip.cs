using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpellTooltip : MonoBehaviour
{
    private Text tooltip;
    private RectTransform background;

    private InputAction mousePosAction;
    private Vector2 mousePosition;

    private void OnEnable()
    {
        Repository.Get<InputAction>(References.Inputs).Enable();
        mousePosAction.performed += OnMouseMove;
    }

    private void OnDisable()
    {
        mousePosAction.performed -= OnMouseMove;
    }

    void Start()
    {
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        mousePosAction = inputs["Core/MousePosition"];



        tooltip = Repository.Get<Text>(References.TooltipText);
        background = Repository.Get<RectTransform>(References.TooltipBackground);

        ShowTooltip("Random Tooltip");
    }

    private void Update()
    {
        Debug.Log(mousePosition);
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent, mousePosition, )
    }

    void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);

        tooltip.text = tooltipString;
        float textPadding = 4f;
        Vector2 bgSize = new Vector2(tooltip.preferredWidth + textPadding, tooltip.preferredHeight + textPadding);
        background.sizeDelta = bgSize;
    }

    void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2Int>();
    }
}
