using System;
using Flux;
using Flux.Event;
using Flux.Feedbacks;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    [SerializeField] private Sequencer sequencer;
    [SerializeField] private Tileable tileable;

    private SendbackArgs args;
    
    protected override void Awake()
    {
        base.Awake();
        
        args = new SendbackArgs();
        args.onDone += OnFeedbackDone;
    }

    protected override void OnLogicDone()
    {
        if (tileable.IsMoving) tileable.PauseMove();
        
        if (sequencer.IsPlaying && sequencer.Args is ISendback sendback) sendback.End(EventArgs.Empty);
        sequencer.Play(args);
    }

    void OnFeedbackDone(EventArgs args)
    {
        tileable.ResumeMove();
        EndFeedback();
    }
}