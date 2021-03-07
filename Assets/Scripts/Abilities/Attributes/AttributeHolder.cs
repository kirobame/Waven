using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Event;
using UnityEngine;

public class AttributeHolder : MonoBehaviour, IAttributeHolder
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
    
    public void Add(CastArgs args)
    {
        if (registry.ContainsKey(args.Id)) return;
        
        args.Initialize(this);
        registry.Add(args.Id, args);
        
        Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
    }
    public bool Remove(CastArgs args)
    {
        if (registry.Remove(args.Id))
        {
            Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
            return true;
        }
        else return false;
    }
}