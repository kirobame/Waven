using System.Collections.Generic;
using Flux;
using UnityEngine;

public class TempCaster : MonoBehaviour, ITempCaster
{
    public IReadOnlyDictionary<Id, CastArgs> Args => registry;
    
    [SerializeReference] private CastArgs[] args = new CastArgs[0];

    private Dictionary<Id, CastArgs> registry = new Dictionary<Id, CastArgs>();

    void Awake()
    {
        foreach (var arg in args)
        {
            if (registry.ContainsKey(arg.Id)) continue;
            registry.Add(arg.Id, arg);
        }
    }
}