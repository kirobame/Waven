using System;
using Flux;
using UnityEngine;

[Serializable]
public abstract class CastArgs
{
    public Id Id => id;
    [SerializeField] protected Id id;

    protected IAttributeHolder owner;

    public virtual void Initialize(IAttributeHolder owner) => this.owner = owner;

    public void SetId(Id id) => this.id = id;

    public abstract CastArgs Copy();
}