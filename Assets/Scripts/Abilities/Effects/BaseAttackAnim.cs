using Flux;
using Flux.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseAttackAnim : Effect
{
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, CastArgs> args)
    {
        Events.Open(GameEvent.OnBaseAttack);
        Events.EmptyCall(GameEvent.OnBaseAttack);
    }
}
