using System;
using Flux;
using UnityEngine;

[Serializable]
public abstract class CastArgs
{
    public Id Id => id;
    [SerializeField] private Id id;
}