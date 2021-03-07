using System;
using System.Collections.Generic;
using Flux;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SpellBase : SerializedScriptableObject, ICastable
{
    public event Action onCastDone;
    
    public abstract bool IsDone { get; }
    public abstract bool ConsumeSpellRemainingUse { get; }
    public abstract IReadOnlyList<Pattern> CastingPatterns { get; }

    public SpellCategory Category => category;
    [SerializeField] private SpellCategory category;

    public Id Id => id;
    [SerializeField] private Id id;
    
    public string Title => title;
    [Space, SerializeField] private string title;

    public string Description => description;
    [SerializeField, TextArea] private string description;

    public Sprite Thumbnail => thumbnail;
    [SerializeField] private Sprite thumbnail;
    
    public abstract void Prepare();

    public abstract bool CanBeCasted(IReadOnlyDictionary<Id, CastArgs> args);

    public abstract HashSet<Tile> GetTilesForCasting(Tile source, IReadOnlyDictionary<Id, CastArgs> args);
    public abstract HashSet<Tile> GetAffectedTilesFor(Tile source, IReadOnlyDictionary<Id, CastArgs> args);

    public abstract void CastFrom(Tile source, IReadOnlyDictionary<Id, CastArgs> args);
    protected void EndCast() => onCastDone?.Invoke();
}