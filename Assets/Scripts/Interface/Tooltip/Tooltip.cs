using System;
using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using Flux.Event;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private RectTransform RectTransform => (RectTransform)transform;
    [SerializeField] private TMP_Text tooltip;

    void Awake()
    {
        Events.Open(InterfaceEvent.OnTooltipUsed);
        
        Events.Register(InterfaceEvent.OnTooltipUsed, OnUsed);
        Events.RelayByVoid(GameEvent.OnTurnStart, HideTooltip);
        
        ShowTooltip("Random Tooltip");
    }

    void OnDestroy()
    {
        Events.Unregister(InterfaceEvent.OnTooltipUsed, OnUsed);
        Events.BreakVoidRelay(GameEvent.OnTurnStart, HideTooltip);
    }

    public void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);
        
        tooltip.text = tooltipString;
        var bgSize = new Vector2(300, tooltip.preferredHeight + 20);
        
        RectTransform.sizeDelta = bgSize;

        Canvas.ForceUpdateCanvases();
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    void OnUsed(EventArgs args)
    {
        if (args is WrapperArgs<string> stringArgs) ShowTooltip(stringArgs.ArgOne);
        else HideTooltip();
    }
}
