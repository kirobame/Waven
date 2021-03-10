using System;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;

[Serializable]
public class MovementBoost : MutableBoost<Moveable>
{
    public override Id Id => new Id('M','V','T');
    public override StatType Type => StatType.Movement;
}