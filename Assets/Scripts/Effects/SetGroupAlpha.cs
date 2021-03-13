using System;
using Flux;
using UnityEngine;

[Serializable, Path("UI")]
public class SetGroupAlpha : Flux.Feedbacks.Effect
{
    [SerializeField] private CanvasGroup target;

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

    private void Execute(float ratio) => target.alpha = Mathf.Lerp(start, end, curve.Evaluate(ratio));
}