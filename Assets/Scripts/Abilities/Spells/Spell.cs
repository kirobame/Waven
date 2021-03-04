using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Waven/Spell")]
public class Spell : SpellBase
{
    public override bool IsDone => isDone;
    public override IReadOnlyList<Pattern> CastingPatterns => castingPatterns;
    
    [Space, SerializeField] private List<Pattern> castingPatterns = new List<Pattern>();
    [SerializeField] private List<Effect> effects = new List<Effect>();

    private int lastingEffects;
    private bool isDone;
    
    //------------------------------------------------------------------------------------------------------------------/

    public override void Prepare() => isDone = false;

    public override HashSet<Tile> GetAffectedTilesFor(Tile source)
    {
        var tiles = new HashSet<Tile>();
        foreach (var effect in effects) tiles.UnionWith(effect.Patterns.Accumulate(source));

        return tiles;
    }
    public override void CastFrom(Tile source)
    {
        isDone = true;
        foreach (var effect in effects)
        {
            lastingEffects++;
            effect.onDone += OnEffectDone;
            
            effect.PlayOn(source);
        }
    }

    void OnEffectDone(Effect effect)
    {
        effect.onDone -= OnEffectDone;
        
        lastingEffects--;
        if (lastingEffects == 0) EndCast();
    }
}
