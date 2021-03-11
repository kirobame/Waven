using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Flux.Audio;
using Flux.Data;

public class EndTurnButton : GameButton, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioHandler.Play(Repository.Get<AudioClipPackage>(AudioReferences.MouseHoverClickableUI));
    }

    protected override void OnClick(PointerEventData eventData)
    {
        AudioHandler.Play(Repository.Get<AudioClipPackage>(AudioReferences.MouseClickOnClickableUI));
        Player.Active.OnEndTurn();
    }
}


