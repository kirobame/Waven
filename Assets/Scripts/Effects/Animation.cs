using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using UnityEngine;

[Serializable, Path("Animations")] // Specifies the creation path like a MenuItem once in the SequenceEditor
public class Animation : Flux.Feedbacks.Effect
{
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName;
    [SerializeField] private string outTag;

    private bool triggerActivated;
        
    //---[Core]-----------------------------------------------------------------------------------------------------/

    public override void Ready()
    {
        base.Ready();
        triggerActivated = false; // Before each sequence execution, reset this state to execute the trigger only once
    }

        
    // Update is the first flow method to be called on an effect
    // As long as IsDone is true, OnUpdate will be called each tick and all linked effects will not receive any flow
    protected override void OnUpdate(EventArgs args)
    {
        if (!triggerActivated) // Execute the trigger
        {
            animator.SetTrigger(triggerName);
            triggerActivated = true;

            return;
        }

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Check if we have reached the given Out state
        if (stateInfo.IsTag(outTag)) IsDone = true; // If so, release control from this effect & let the flow trickle down deeper
    }
}