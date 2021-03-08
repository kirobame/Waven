using System.Collections.Generic;
using Flux.Data;
using Flux.Event;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    #region Nested Types

    private enum Activity
    {
        None,
        Hover,
        Global
    }

    #endregion

    private GenericPool infoPool;

    private bool hasHoverInfo;
    private EntityInfo hoverInfo;
    
    private List<EntityInfo> spellInfos = new List<EntityInfo>();
    
    private Activity activity;
    
    void Awake()
    {
        Events.Open(InterfaceEvent.OnInfoRefresh);
        Events.Open(InterfaceEvent.OnHoverStart);
        Events.Open(InterfaceEvent.OnHoverEnd);
        Events.Open(InterfaceEvent.OnSpellTilesAffect);
        Events.Open(InterfaceEvent.OnSpellEnd);
        
        Events.RelayByValue<InfoAnchor,GameObject>(InterfaceEvent.OnHoverStart, OnHoverStart);
        Events.RelayByVoid(InterfaceEvent.OnHoverEnd, OnHoverEnd);
        Events.RelayByValue<Spellcaster>(InterfaceEvent.OnSpellTilesAffect, OnSpellTilesAffect);
        Events.RelayByVoid(InterfaceEvent.OnSpellEnd, Shutdown);
    }
    void Start() => infoPool = Repository.Get<GenericPool>(Pools.Info);
    
    void OnDestroy()
    {
        Events.BreakValueRelay<InfoAnchor,GameObject>(InterfaceEvent.OnHoverStart, OnHoverStart);
        Events.BreakVoidRelay(InterfaceEvent.OnHoverEnd, OnHoverEnd);
        Events.BreakValueRelay<Spellcaster>(InterfaceEvent.OnSpellTilesAffect, OnSpellTilesAffect);
        Events.BreakVoidRelay(InterfaceEvent.OnSpellEnd, Shutdown);
    }

    private EntityInfo SetupInfoFor(InfoAnchor anchor, GameObject source)
    {
        var instance = infoPool.CastSingle<EntityInfo>();
        
        instance.RectTransform.SetParent(transform);
        instance.AssignTo(anchor, source);

        return instance;
    }
    
    void OnHoverStart(InfoAnchor anchor, GameObject source)
    {
        hasHoverInfo = true;
        hoverInfo = SetupInfoFor(anchor, source);
    }
    void OnHoverEnd()
    {
        if (hoverInfo != null) hoverInfo.gameObject.SetActive(false);
        hoverInfo = null;
        
        hasHoverInfo = false;
    }

    void OnSpellTilesAffect(Spellcaster caster)
    {
        HoverSignal.activeId = int.MaxValue;
        if (hasHoverInfo) OnHoverEnd();

        foreach (var tile in caster.AvailableTiles)
        {
            foreach (var entity in tile.Entities)
            {
                var component = (Component)entity;
                if (!component.TryGet<InfoAnchor>(out var anchor)) continue;
                
                spellInfos.Add(SetupInfoFor(anchor, component.gameObject));
            }
        }
    }
    
    void Shutdown()
    {
        if (hasHoverInfo) OnHoverEnd();
        
        HoverSignal.activeId = 0;

        foreach (var spellInfo in spellInfos) spellInfo.gameObject.SetActive(false);
        spellInfos.Clear();
    }
}