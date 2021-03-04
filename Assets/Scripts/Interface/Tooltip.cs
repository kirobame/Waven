using System;
using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using Flux.Event;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private RectTransform rectTransform => (RectTransform)transform;

    [SerializeField] private float screenMargin;
    [SerializeField] private Text tooltip;

    private bool onUsed;

    void Awake()
    {
        Events.Open(InterfaceEvent.OnTooltipUsed);
        Events.Register(InterfaceEvent.OnTooltipUsed, OnUsed);
        
        ShowTooltip("Random Tooltip");
    }

    void Update()
    {
        if (!onUsed) return;
        
        var mousePosition = new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y);
        var tooltipPosition = mousePosition.x + rectTransform.sizeDelta.x;
        
        if(tooltipPosition > Screen.width - screenMargin)
        {
            var offset = tooltipPosition - (Screen.width - screenMargin);
            rectTransform.position = mousePosition + Vector2.right * offset;
        }
        else rectTransform.position = mousePosition;
    }

    public void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);
        tooltip.text = tooltipString;
        
        var textPadding = 4f;
        var bgSize = new Vector2(tooltip.preferredWidth + textPadding, tooltip.preferredHeight + textPadding);
        rectTransform.sizeDelta = bgSize;
    }
    public void HideTooltip() => gameObject.SetActive(false);

    void OnUsed(EventArgs args)
    {
        if (args is WrapperArgs<string> stringArgs)
        {
            onUsed = true;
            ShowTooltip(stringArgs.ArgOne);
            
        }
        else
        {
            onUsed = false;
            HideTooltip();
        }
    }
}
