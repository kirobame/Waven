using System;
using Flux;
using UnityEngine;

[Serializable, Path("Animations")]
public class ResetTrigger : Flux.Feedbacks.Effect
{
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName;

    protected override void OnUpdate(EventArgs args)
    {
        animator.ResetTrigger(triggerName);
        IsDone = true;
    }
}