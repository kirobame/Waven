using System;
using Flux.Data;
using Flux.Event;
using Flux.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<PointerHandler> onEnter;
    public event Action<PointerHandler> onExit;

    public SpellBase Spell => relay.Value;
    [SerializeField] private SpellHolder relay;

    [SerializeField] private RectTransform tooltipPos;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke(this);

        AudioHandler.Play(Repository.Get<AudioClipPackage>(AudioReferences.MouseHoverClickableUI));

        var message = $"<size=100%><b>{Spell.Title}</size></b>\n<size=65%>{Spell.Description}</size>";
        Events.ZipCall(InterfaceEvent.OnTooltipUsed, message, tooltipPos, 2);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke(this);
        Events.EmptyCall(InterfaceEvent.OnHideTooltip);
    }
}
