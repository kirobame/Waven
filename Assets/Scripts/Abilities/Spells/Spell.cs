using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

public enum SpellCategory
{
    Neutral,
    Iop,
    Sram,
    Osamodas
}

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

    public override void Prepare()
    {
        lastingEffects = 0;
        isDone = false;
    }

    public override bool CanBeCasted(IReadOnlyDictionary<Id, CastArgs> args) => effects.All(effect => effect.CanBeCasted(args));

    public override HashSet<Tile> GetTilesForCasting(Tile source, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var output = new HashSet<Tile>();
        foreach (var pattern in castingPatterns) output.UnionWith(pattern.GetTiles(source, args));

        return output;
    }
    public override HashSet<Tile> GetAffectedTilesFor(Tile source, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var tiles = new HashSet<Tile>();
        foreach (var effect in effects) tiles.UnionWith(effect.GetAffectedTiles(source, args));

        return tiles;
    }
    
    public override void CastFrom(Tile source, IReadOnlyDictionary<Id, CastArgs> args)
    {
        isDone = true;

        foreach (var effect in effects)
        {
            lastingEffects++;
            effect.onDone += OnEffectDone;
        }
        
        foreach (var effect in effects) effect.PlayOn(source, args);
    }

    void OnEffectDone(Effect effect)
    {
        effect.onDone -= OnEffectDone;
        
        lastingEffects--;
        if (lastingEffects == 0) EndCast();
    }
}
