using System;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using Flux.Event;

[Serializable]
public class AttackBoost : MutableBoost<Pathfinder>
{
    public override Id Id => new Id('A','T','K');
    public override StatType Type => StatType.Damage;
}