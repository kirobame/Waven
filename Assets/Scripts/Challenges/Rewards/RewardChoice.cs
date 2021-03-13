using System;
using Flux.Data;
using Flux.Event;
using Flux.Audio;
using Flux.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardChoice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RewardHandler handler;
    [SerializeField] private SpellCategory category;

    [Space, SerializeField] private Sequencer hide;
    [SerializeField] private SimpleButton button;
    
    [Space, SerializeField] private TMP_Text title;
    [SerializeField] private Image thumbnail;
    [SerializeField] private AudioClipPackage spellChoosenSound;

    private SpellBase spell;
    private int tier;

    [SerializeField] private RectTransform tooltipPos;

    void Awake() => button.onClick += OnClick;
    void OnDestroy() => button.onClick -= OnClick;
    
    public void TryHide(SpellBase spell)
    {
        if (this.spell == spell) return;

        button.IsOperational = false;
        hide.Play(EventArgs.Empty);
    }
    
    public void Initialize(int tier)
    {
        var key = new SpellKey(category, tier);
        var spells = Repository.Get<Spells>(References.Spells);

        spell = spells.GetRandom(key);
        title.text = spell.Title;
        thumbnail.sprite = spell.Thumbnail;

        button.IsOperational = true;
        
        var color = thumbnail.color;
        color.a = 0.8f;
        thumbnail.color = color;
        
        thumbnail.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 122.5f);
        thumbnail.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 122.5f);
    }

    public void OnClick(SimpleButton button)
    {
        AudioHandler.Play(spellChoosenSound);
        AudioHandler.Play(Repository.Get<AudioClipPackage>(AudioReferences.MouseClickOnClickableUI));
        handler.Pick(spell);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioHandler.Play(Repository.Get<AudioClipPackage>(AudioReferences.MouseHoverClickableUI));
        var message = $"<size=100%><b>{spell.Title}</size></b>\n<color=#f7f7f7><size=65%>{spell.Description}</size></color>";
        Events.ZipCall(InterfaceEvent.OnTooltipUsed, message, tooltipPos, 1);
    }
    public void OnPointerExit(PointerEventData eventData) => Events.EmptyCall(InterfaceEvent.OnHideTooltip);
}