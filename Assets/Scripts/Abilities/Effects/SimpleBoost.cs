using System;
using UnityEngine;

[Serializable]
public class SimpleBoost : Boost
{
    public override StatType Type => type;

    [SerializeField] private CastArgs boost;
    [SerializeField] private StatType type;

    public override CastArgs GetBoost() => boost.Copy();
}