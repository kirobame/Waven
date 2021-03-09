using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Event;
using UnityEngine;

public class AttributeHolder : MonoBehaviour, IAttributeHolder
{
    public IReadOnlyDictionary<Id, List<CastArgs>> Args => registry;
    [SerializeReference] private CastArgs[] args = new CastArgs[0];

    public PopupAnchor PopupAnchor => popupAnchor;
    [SerializeField] private PopupAnchor popupAnchor;

    private Dictionary<Id, List<CastArgs>> registry = new Dictionary<Id, List<CastArgs>>();

    void Awake() { foreach (var arg in args) AddToRegistry(arg); }
    
    public void Add(CastArgs args)
    {
        AddToRegistry(args);
        Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
    }
    private void AddToRegistry(CastArgs arg)
    {
        arg.Initialize(this);
            
        if (registry.ContainsKey(arg.Id)) registry[arg.Id].Add(arg);
        else registry.Add(arg.Id, new List<CastArgs>() { arg });
    }
    
    public bool Remove(CastArgs args)
    {
        if (!registry.ContainsKey(args.Id)) return false;
        
        registry[args.Id].Remove(args);
        if (!registry[args.Id].Any()) registry.Remove(args.Id);
        
        Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
        return true;
    }
    public bool RemoveAll(Id id)
    {
        if (registry.Remove(id))
        {
            Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
            return true;
        }
        else return false;
    }
}