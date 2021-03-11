using System;
using Flux;
using Flux.Event;
using Flux.Feedbacks;
using UnityEngine;

public class EntityDamageable : Damageable
{
    [SerializeField] private Sequencer hitSequencer;
    [SerializeField] private Sequencer deathSequencer;
    [SerializeField] private Sequencer fallSequencer;
    
    [Space, SerializeField] private Tileable tileable;

    private SendbackArgs args;
    private SendbackArgs deathArgs;
    
    protected override void Awake()
    {
        base.Awake();
        
        args = new SendbackArgs();
        args.onDone += OnFeedbackDone;
        
        deathArgs = new SendbackArgs();
        deathArgs.onDone += OnDeathFeedbackDone;
    }

    protected override void OnLogicDone()
    {
        if (tileable.IsMoving) tileable.PauseMove();
        BeginSequence(hitSequencer, args);
    }
    protected override void OnDeath()
    {
        if (tileable is Player player) Events.ZipCall(GameEvent.OnPlayerDeath, player);

        if (tileable.Navigator.Current is DeathTile deathTile)
        {
            FallArgs args;

            if (deathTile.IsTop) args = new FallArgs("Default", -2);
            else args = new FallArgs("Entities", 2);
            
            args.onDone += OnDeathFeedbackDone;
            BeginSequence(fallSequencer, args);
        }
        else BeginSequence(deathSequencer, deathArgs);
    }

    private void BeginSequence(Sequencer sequencer, EventArgs args)
    {
        if (sequencer.IsPlaying && sequencer.Args is ISendback sendback) sendback.End(EventArgs.Empty);
        sequencer.Play(args);
    }
    
    void OnFeedbackDone(EventArgs args)
    {
        tileable.ResumeMove();
        EndFeedback();
    }
    void OnDeathFeedbackDone(EventArgs args) => base.OnDeath();
}