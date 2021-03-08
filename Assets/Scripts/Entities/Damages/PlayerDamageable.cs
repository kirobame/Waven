using System;
using Flux.Event;
using Flux.Feedbacks;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    [SerializeField] private Sequencer sequencer;

    private SendbackArgs args;
    
    protected override void Awake()
    {
        base.Awake();
        
        args = new SendbackArgs();
        args.onDone += OnFeedbackDone;
    }

    protected override void OnLogicDone() => sequencer.Play(args);

    void OnFeedbackDone(EventArgs args) => EndFeedback();
}