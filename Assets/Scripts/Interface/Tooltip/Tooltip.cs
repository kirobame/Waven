﻿using System;
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

    [SerializeField] private float screenMargin;
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
    
    private void Place()
    {
        var position = Mouse.current.position.ReadValue();

        var min = position;
        var max = position + RectTransform.sizeDelta;

        var xMax = Screen.width - screenMargin;
        if (max.x > xMax)
        {
            var difference = max.x - xMax;
            position.x -= difference;
        }
        else if (min.x < screenMargin) position.x = screenMargin;

        var yMax =  Screen.height - screenMargin;
        if (max.y > yMax)
        {
            var difference = max.y - yMax;
            position.y -= difference;
        }
        else if (min.y < screenMargin) position.y = screenMargin;

        RectTransform.position = position;
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
