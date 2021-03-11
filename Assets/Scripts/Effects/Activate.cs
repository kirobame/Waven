using System;
using Flux;
using UnityEngine;

[Serializable, Path("General")]
public class Activate : Flux.Feedbacks.Effect
{
    [SerializeField] private Component component;
    [SerializeField] private bool value;
    
    protected override void OnUpdate(EventArgs args)
    {
        component.gameObject.SetActive(value);
        IsDone = true;
    }
}