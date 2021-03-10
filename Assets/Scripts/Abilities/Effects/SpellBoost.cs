using System;
using Flux;

[Serializable]
public class SpellBoost : MutableBoost<Spellcaster>
{
    public override Id Id => new Id('S','P','L');
    public override StatType Type => StatType.Spells;
}