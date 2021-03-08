using System.Collections;
using Flux;
using UnityEngine;

public class Bomb : Damageable
{
    [SerializeField] private TileableBase tileable;
    [SerializeField] private Spell spell;
    [SerializeField] private SpriteRenderer graph;
    
    protected override void OnDeath()
    {
        graph.enabled = false;
        StartCoroutine(WaitRoutine());
    }

    private IEnumerator WaitRoutine()
    {
        for (var i = 0; i < 2; i++) yield return new WaitForEndOfFrame();
        
        spell.Prepare();
        spell.onCastDone += Die;
        spell.CastFrom(tileable.Navigator.Current, Spellcaster.EmptyArgs);
    }

    void Die()
    {
        spell.onCastDone -= Die;
        EndFeedback();
        
        base.OnDeath();
    }
}