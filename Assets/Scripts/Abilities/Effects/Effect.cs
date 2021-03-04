using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using Flux.Editor;
using Sirenix.Utilities;
using UnityEngine;

public abstract class Effect
{
    public IReadOnlyList<Pattern> Patterns => patterns;
    [SerializeField] protected Pattern[] patterns = new Pattern[0];

    public void PlayOn(Tile source)
    {
        var tiles = patterns.Accumulate(source);
        ApplyTo(tiles);
    }

    protected abstract void ApplyTo(IEnumerable<Tile> tiles);
}