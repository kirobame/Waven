using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EndTurnButton : GameButton
{
    protected override void OnClick(PointerEventData eventData)
    {
        Player.Active.OnEndTurn();
    }
}
