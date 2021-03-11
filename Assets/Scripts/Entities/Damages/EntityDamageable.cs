using System;
using Flux;
using Flux.Event;
using Flux.Feedbacks;
using UnityEngine;

public class EntityDamageable : Damageable
{
    [SerializeField] protected Sequencer hitSequencer;
    [SerializeField] protected Sequencer deathSequencer;
    [SerializeField] protected Sequencer fallSequencer;
    
    [Space, SerializeField] protected Tileable tileable;

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

    protected void BeginSequence(Sequencer sequencer, EventArgs args)
    {
        if (sequencer.IsPlaying && sequencer.Args is ISendback sendback) sendback.End(EventArgs.Empty);
        sequencer.Play(args);
    }
    
    protected void OnFeedbackDone(EventArgs args)
    {
        tileable.ResumeMove();
        EndFeedback();
    }
    protected void OnDeathFeedbackDone(EventArgs args) => base.OnDeath();
}