using Flux.Data;
using Flux.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SpellTooltip tooltip;
    private UISpellData spellData;

    private void Start()
    {
        tooltip = Repository.Get<RectTransform>(References.TooltipBackground).GetComponent<SpellTooltip>();
        spellData = gameObject.GetComponent<UISpellData>();
    }

    private void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Events.RelayByValue<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
        string tooltipMessage = spellData.spell.Title + "\n" + spellData.spell.Description;
        tooltip.ShowTooltip(tooltipMessage);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Events.BreakValueRelay<Vector2>(InputEvent.OnMouseMove, OnMouseMove);
        tooltip.HideTooltip();
    }

    void OnMouseMove(Vector2 mousePosition)
    {
        Debug.Log(mousePosition);
        tooltip.transform.position = mousePosition;
    }
}
