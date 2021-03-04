using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using Flux.Editor;
using Sirenix.Utilities;
using UnityEngine;

public abstract class Effect
{
    public event Action<Effect> onDone;
    
    public IReadOnlyList<Pattern> Patterns => patterns;
    [SerializeField] protected Pattern[] patterns = new Pattern[0];

    public virtual bool CanBeCasted(IReadOnlyDictionary<Id, CastArgs> args) => true;
    
    public void PlayOn(Tile source, IReadOnlyDictionary<Id, CastArgs> args)
    {
        var tiles = patterns.Accumulate(source);
        ApplyTo(source, tiles, args);
    }

    protected abstract void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args);
    protected void End() => onDone?.Invoke(this);
}