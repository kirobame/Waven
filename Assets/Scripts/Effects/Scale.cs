using System;
using Flux;
using UnityEngine;

[Serializable, Path("UI")]
public class Scale : Flux.Feedbacks.Effect
{
    [SerializeField] private RectTransform target;

    [SerializeField] private float time;
    [SerializeField] private AnimationCurve curve;

    [SerializeField] private float startX;
    [SerializeField] private float startY;
    [SerializeField] private float endX;
    [SerializeField] private float endY;

    private Vector2 start => new Vector2(startX, startY);
    private Vector2 end => new Vector2(endX, endY);
    
    private float counter;
    
    public override void Ready()
    {
        base.Ready();
        counter = 0.0f;
    }

    protected override void OnUpdate(EventArgs args)
    {
        if (counter >= time)
        {
            Execute(1.0f);
            IsDone = true;

            return;
        }
        
        counter += Time.deltaTime;
        Execute(counter / time);
    }

    private void Execute(float ratio)
    {
        var size = Vector2.Lerp(start, end, curve.Evaluate(ratio));
        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
    }
}