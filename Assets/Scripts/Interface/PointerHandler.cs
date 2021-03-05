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
        var message = $"<size=100%><b>{data.actualSpell.Title}</size></b>\n<size=65%>{data.actualSpell.Description}</size>";
        Events.ZipCall(InterfaceEvent.OnTooltipUsed, message);
    }

    public void OnPointerExit(PointerEventData eventData) => Events.EmptyCall(InterfaceEvent.OnTooltipUsed);
}
