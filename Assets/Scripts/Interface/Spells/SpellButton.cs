using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using Flux.Data;
using Flux.Audio;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpellButton : GameButton
{
    public SpellBase Spell => relay.Value;
    [SerializeField] protected SpellHolder relay;

    private Spellcaster Spellcaster => Player.Active.GetComponent<Spellcaster>();
    
    protected override void OnClick(PointerEventData eventData)
    {
        if (Spellcaster.RemainingUse <= 0)
        {
            AudioHandler.Play(Repository.Get<AudioClipPackage>(AudioReferences.MouseClickOnLockedUI));
            return;
        }

        if (Spellcaster.Active != null && Spellcaster.Active.Current == Spell) Events.Call(InputEvent.OnInterrupt, new WrapperArgs<bool>(true));
        else
        {
            AudioHandler.Play(Repository.Get<AudioClipPackage>(AudioReferences.MouseClickOnClickableUI));
            Events.ZipCall<SpellBase, bool>(InterfaceEvent.OnSpellSelected, Spell, false);
        }
    }
}