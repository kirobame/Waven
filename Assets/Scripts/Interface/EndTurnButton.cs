using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public abstract class GameButton : MonoBehaviour, IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!Buffer.isGameTurn) return;
        OnClick(eventData);
    }
    protected abstract void OnClick(PointerEventData data);
}

public class EndTurnButton : GameButton
{
    protected override void OnClick(PointerEventData eventData)
    {
        Player.Active.OnEndTurn();
    }
}
