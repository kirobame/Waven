using System;
using Flux;
using UnityEngine;

[Serializable, Path("General")]
public class Enable : Flux.Feedbacks.Effect
{
    [SerializeField] private Behaviour component;
    [SerializeField] private bool value;
    
    protected override void OnUpdate(EventArgs args)
    {
        component.enabled = value;
        IsDone = true;
    }
}