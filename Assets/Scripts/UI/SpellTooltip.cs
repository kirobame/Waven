using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpellTooltip : MonoBehaviour
{
    private Text tooltip;
    private RectTransform background;

    [SerializeField] public float screenMargin;

    void Start()
    {
        tooltip = Repository.Get<Text>(References.TooltipText);
        background = Repository.Get<RectTransform>(References.TooltipBackground);

        ShowTooltip("Random Tooltip");
    }

    public void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);

        tooltip.text = tooltipString;
        float textPadding = 4f;
        Vector2 bgSize = new Vector2(tooltip.preferredWidth + textPadding, tooltip.preferredHeight + textPadding);
        background.sizeDelta = bgSize;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
