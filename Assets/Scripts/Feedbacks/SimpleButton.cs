using System;
using Flux.Event;
using Flux.Feedbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<SimpleButton> onClick;
    public event Action<SimpleButton> onHoverStart;
    public event Action<SimpleButton> onHoverEnd;

    public bool IsOperational { get; set; }

    [SerializeField] private bool isUnscaled;
    
    [Space, SerializeField] private Timetable table;
    [SerializeField] private Sequencer click;

    private SendbackArgs clickArgs;
    private bool hasHover;

    protected virtual void Awake()
    {
        IsOperational = true;
        
        clickArgs = new SendbackArgs();
        clickArgs.onDone += OnClickDone;

        hasHover = false;
    }

    void Update()
    {
        if (!IsOperational) return;

        var delta = isUnscaled ? Time.unscaledDeltaTime : Time.deltaTime;
        if (hasHover) table.MoveBy(delta);
        else table.MoveBy(-delta);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsOperational) return;
        
        onClick?.Invoke(this);
        OnClick();

        IsOperational = false;
        click.Play(clickArgs);
    }
    protected virtual void OnClick() { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hasHover = true;
        onHoverStart?.Invoke(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hasHover = false;
        onHoverEnd?.Invoke(this);
    }

    void OnClickDone(EventArgs args) => IsOperational = true;
}