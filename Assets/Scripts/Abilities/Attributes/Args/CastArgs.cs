using System;
using Flux;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public abstract class CastArgs : IEquatable<CastArgs>
{
    public Id Id => id;
    [SerializeField] protected Id id;

    protected IAttributeHolder owner;
    private float time;

    public virtual void Initialize(float time, IAttributeHolder owner)
    {
        this.time = Random.Range(0,1000) * time;
        this.owner = owner;
    }

    public void SetId(Id id) => this.id = id;

    public abstract CastArgs Copy();

    public override int GetHashCode() => time.GetHashCode();

    public bool Equals(CastArgs other) => other.time == time;
    public override bool Equals(object obj) => obj is CastArgs other && other.time == time;
}