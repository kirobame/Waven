using System;
using Flux;
using UnityEngine;

[Serializable]
public abstract class CastArgs
{
    public Id Id => id;
    [SerializeField] private Id id;

    protected IAttributeHolder owner;

    public virtual void Initialize(IAttributeHolder owner) => this.owner = owner;

    public abstract CastArgs Copy();
}