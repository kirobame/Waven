using Flux.Data;
using Flux.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SpellButton data;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        var tooltipMessage = data.actualSpell.Title + "\n" + data.actualSpell.Description;
        Events.ZipCall(InterfaceEvent.OnTooltipUsed, tooltipMessage);
    }

    public void OnPointerExit(PointerEventData eventData) => Events.EmptyCall(InterfaceEvent.OnTooltipUsed);
}
