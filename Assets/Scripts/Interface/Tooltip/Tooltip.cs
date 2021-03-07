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

    [SerializeField] private RectTransform canvas;
    
    [Space, SerializeField] private float screenMargin;
    [SerializeField] private TMP_Text tooltip;

    private bool onUsed;

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

    void Update()
    {
        if (!onUsed) return;
        Place();
    }

    private void Place()
    {
        var position = Mouse.current.position.ReadValue();
        var max = position + RectTransform.sizeDelta;

        var xMax = canvas.sizeDelta.x - screenMargin;
        if (max.x > xMax) position.x += max.x - xMax;

        var yMax = canvas.sizeDelta.y - screenMargin;
        if (max.y > yMax) position.y += max.y - yMax;

        RectTransform.position = position;
    }

    public void ShowTooltip(string tooltipString)
    {
        onUsed = true;
        gameObject.SetActive(true);
        
        tooltip.text = tooltipString;
        Canvas.ForceUpdateCanvases();
        var bgSize = new Vector2(tooltip.preferredWidth, tooltip.preferredHeight);
        
        RectTransform.sizeDelta = bgSize;
        Place();
    }
    public void HideTooltip()
    {
        onUsed = false;
        gameObject.SetActive(false);
    }

    void OnUsed(EventArgs args)
    {
        if (args is WrapperArgs<string> stringArgs) ShowTooltip(stringArgs.ArgOne);
        else HideTooltip();
    }
}
