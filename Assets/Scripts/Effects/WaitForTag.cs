using System;
using Flux;
using UnityEngine;

[Serializable, Path("Animations")] // Specifies the creation path like a MenuItem once in the SequenceEditor
public class WaitForTag : Flux.Feedbacks.Effect
{
    [SerializeField] private Animator animator;
    [SerializeField] private string name;

    protected override void OnUpdate(EventArgs args)
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0); 
        if (stateInfo.IsTag(name)) IsDone = true; 
    }
}