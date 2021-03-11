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

    [SerializeField] private float screenMargin;
    [SerializeField] private TMP_Text tooltip;

    void Awake()
    {
        Events.Open(InterfaceEvent.OnTooltipUsed);
        
        Events.RelayByValue<string, RectTransform, int>(InterfaceEvent.OnTooltipUsed, OnUsed);
        Events.RelayByVoid(InterfaceEvent.OnHideTooltip, HideTooltip);
        Events.RelayByVoid(GameEvent.OnTurnStart, HideTooltip);

        var tr = new RectTransform();
        ShowTooltip("Random Tooltip", tr, 0);
    }

    void OnDestroy()
    {
        Events.BreakValueRelay<string, RectTransform, int>(InterfaceEvent.OnTooltipUsed, OnUsed);
        Events.BreakVoidRelay(InterfaceEvent.OnHideTooltip, HideTooltip);
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

    public void ShowTooltip(string tooltipString, RectTransform transform, int index)
    {
        gameObject.SetActive(true);

        switch (index)
        {
            case 1:
                RectTransform.anchorMin = new Vector2(0.5f, 1);
                RectTransform.anchorMax = new Vector2(0.5f, 1);
                RectTransform.pivot = new Vector2(0.5f, 1);

                RectTransform.position = transform.position;
                break;

            case 2:
                RectTransform.anchorMin = new Vector2(0.5f, 0);
                RectTransform.anchorMax = new Vector2(0.5f, 0);
                RectTransform.pivot = new Vector2(0.5f, 0);

                RectTransform.position = transform.position;
                break;
        }
        
        tooltip.text = tooltipString;
        var bgSize = new Vector2(300, tooltip.preferredHeight + 20);
        
        RectTransform.sizeDelta = bgSize;

        Canvas.ForceUpdateCanvases();
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    void OnUsed(string text, RectTransform transform, int index)
    {
        if (text == null) HideTooltip();
        ShowTooltip(text, transform, index);
    }
}
