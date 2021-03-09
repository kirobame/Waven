﻿using Flux.Data;
using Flux.Event;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardChoice : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RewardHandler handler;
    [SerializeField] private SpellCategory category;

    [Space, SerializeField] private TMP_Text title;
    [SerializeField] private Image thumbnail;

    private SpellBase spell;
    private int tier;
    
    public void Initialize(int tier)
    {
        var key = new SpellKey(category, tier);
        var spells = Repository.Get<Spells>(References.Spells);

        spell = spells.GetRandom(key);
        title.text = spell.Title;
        thumbnail.sprite = spell.Thumbnail;
    }
    
    public void OnPointerClick(PointerEventData eventData) => handler.Pick(spell);
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        var message = $"<size=65%>{spell.Description}</size>";
        Events.ZipCall(InterfaceEvent.OnTooltipUsed, message);
    }
    public void OnPointerExit(PointerEventData eventData) => Events.EmptyCall(InterfaceEvent.OnTooltipUsed);
}