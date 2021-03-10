using System;
using Flux;
using Flux.Event;
using Flux.Feedbacks;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    [SerializeField] private Sequencer hitSequencer;
    [SerializeField] private Sequencer deathSequencer;
    [SerializeField] private Sequencer fallSequencer;
    
    [Space, SerializeField] private Player player;

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
        if (player.IsMoving) player.PauseMove();
        BeginSequence(hitSequencer, args);
    }
    protected override void OnDeath()
    {
        Events.ZipCall(GameEvent.OnPlayerDeath, player);
        
        if (player.Navigator.Current is DeathTile) BeginSequence(fallSequencer, deathArgs);
        else BeginSequence(deathSequencer, deathArgs);
    }

    private void BeginSequence(Sequencer sequencer, EventArgs args)
    {
        if (sequencer.IsPlaying && sequencer.Args is ISendback sendback) sendback.End(EventArgs.Empty);
        sequencer.Play(args);
    }
    
    void OnFeedbackDone(EventArgs args)
    {
        player.ResumeMove();
        EndFeedback();
    }
    void OnDeathFeedbackDone(EventArgs args) => base.OnDeath();
}