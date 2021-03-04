using Flux;
using UnityEngine;

public abstract class CastArgs
{
    public Id Id => id;
    [SerializeField] private Id id;
}