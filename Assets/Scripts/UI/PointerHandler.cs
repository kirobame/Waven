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
    private RectTransform rectTooltip;
    private SpellButton spellData;

    private bool pointerOnUI;    

    private void Start()
    {
        rectTooltip = Repository.Get<RectTransform>(References.TooltipBackground);
        tooltip = Repository.Get<RectTransform>(References.TooltipBackground).GetComponent<SpellTooltip>();
        spellData = gameObject.GetComponent<SpellButton>();
    }

    private void Update()
    {
        if (!pointerOnUI) return;

        var tooltipPosition = Mouse.current.position.ReadValue().x + rectTooltip.sizeDelta.x;
        if(tooltipPosition > (Screen.width - tooltip.screenMargin))
        {
            var offset = tooltipPosition - (Screen.width - tooltip.screenMargin);
            tooltip.transform.position = new Vector2((Mouse.current.position.ReadValue().x - offset), Mouse.current.position.ReadValue().y);
        }
        else
        {
            tooltip.transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOnUI = true;
        Debug.Log("Enter");
        string tooltipMessage = spellData.actualSpell.Title + "\n" + spellData.actualSpell.Description;
        tooltip.ShowTooltip(tooltipMessage);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        tooltip.HideTooltip();
        pointerOnUI = false;
    }
}
