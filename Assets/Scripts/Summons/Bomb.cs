using System.Collections;
using Flux;
using Flux.Audio;
using Flux.Event;
using Flux.Feedbacks;
using UnityEngine;

public class Bomb : EntityDamageable
{
    [SerializeField] private AudioClipPackage explosionSound;
    [Space, SerializeField] private Spell spell;
    [SerializeField] private Sequencer explosionSequencer;

    protected override void OnDeath()
    {
        if (tileable.Navigator.Current is DeathTile deathTile)
        {
            FallArgs args;

            if (deathTile.IsTop) args = new FallArgs("Default", -2);
            else args = new FallArgs("Entities", 2);
            
            args.onDone += OnDeathFeedbackDone;
            BeginSequence(fallSequencer, args);
        }
        else
        {
            StartCoroutine(WaitRoutine());
            
            var sendback = new SendbackArgs();
            sendback.onDone += OnDeathFeedbackDone;
            
            BeginSequence(explosionSequencer, sendback);
        }
    }
    
    private IEnumerator WaitRoutine()
    {
        for (var i = 0; i < 2; i++) yield return new WaitForSeconds(0.5f);

        AudioHandler.Play(explosionSound);

        spell.Prepare();
        spell.CastFrom(tileable.Navigator.Current, Spellcaster.EmptyArgs);
    }
}