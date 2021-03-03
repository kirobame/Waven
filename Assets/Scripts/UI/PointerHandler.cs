using Flux.Data;
using Flux.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SpellTooltip tooltip;
    private UISpellData spellData;

    private Vector2 mousePosition;

    private bool pointerOnUI;

    private void Start()
    {
        tooltip = Repository.Get<RectTransform>(References.TooltipBackground).GetComponent<SpellTooltip>();
        spellData = gameObject.GetComponent<UISpellData>();
    }

    private void Update()
    {
        if (!pointerOnUI) return;

        tooltip.transform.position = Mouse.current.position.ReadValue();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOnUI = true;
        Debug.Log("Enter");
        string tooltipMessage = spellData.spell.Title + "\n" + spellData.spell.Description;
        tooltip.ShowTooltip(tooltipMessage);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        tooltip.HideTooltip();
        pointerOnUI = false;
    }
}
