using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class EndTurnButton : MonoBehaviour, IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Player.Active.OnEndTurn();
    }
}
