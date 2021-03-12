using System;
using Flux;
using UnityEngine;

[Serializable, Path("Animations")] // Specifies the creation path like a MenuItem once in the SequenceEditor
public class SetBool : Flux.Feedbacks.Effect
{
    [SerializeField] private Animator animator;
    [SerializeField] private string name;
    [SerializeField] private bool value;
    
    protected override void OnUpdate(EventArgs args)
    {
        animator.SetBool(name, value);
        IsDone = true;
    }
}