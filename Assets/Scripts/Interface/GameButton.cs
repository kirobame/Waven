using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameButton : MonoBehaviour, IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!Buffer.isGameTurn) return;
        OnClick(eventData);
    }
    protected abstract void OnClick(PointerEventData data);
}