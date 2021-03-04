using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SpellBase : SerializedScriptableObject, ICastable
{
    public event Action onCastDone;
    
    public abstract bool IsDone { get; }
    public abstract IReadOnlyList<Pattern> CastingPatterns { get; }
    
    public string Title => title;
    [SerializeField] private string title;

    public string Description => description;
    [SerializeField, TextArea] private string description;

    public Sprite Thumbnail => thumbnail;
    [SerializeField] private Sprite thumbnail;
    
    public abstract void Prepare();

    public abstract HashSet<Tile> GetAffectedTilesFor(Tile source); 
    public abstract void CastFrom(Tile source);

    protected void EndCast() => onCastDone?.Invoke();
}