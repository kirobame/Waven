using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using Flux.Editor;
using Sirenix.Utilities;
using UnityEngine;

public abstract class Effect
{
    public event Action<Effect> onDone;
    
    [SerializeField] protected Pattern[] patterns = new Pattern[0];

    public virtual HashSet<Tile> GetAffectedTiles(Tile source, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        var output = new HashSet<Tile>();
        foreach (var pattern in patterns) output.UnionWith(pattern.GetTiles(source, args));

        return output;
    }
    
    public virtual bool CanBeCasted(IReadOnlyDictionary<Id, List<CastArgs>> args) => true;
    public void PlayOn(Tile source, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        var tiles = GetAffectedTiles(source, args);
        ApplyTo(source, tiles, args);
    }

    protected abstract void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args);
    protected void End() => onDone?.Invoke(this);
}