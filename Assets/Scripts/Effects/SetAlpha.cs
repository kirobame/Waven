using System;
using Flux;
using UnityEngine;
using UnityEngine.UI;

[Serializable, Path("UI")]
public class SetAlpha : Flux.Feedbacks.Effect
{
    [SerializeField] private Image target;

    [SerializeField] private float time;
    [SerializeField] private AnimationCurve curve;

    [SerializeField] private float start;
    [SerializeField] private float end;

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
        var color = target.color;
        color.a = Mathf.Lerp(start, end, curve.Evaluate(ratio));
        target.color = color;
    }
}