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
        
        Events.RelayByValue<InfoAnchor,GameObject>(InterfaceEvent.OnHoverStart, OnHoverStart);
        Events.RelayByVoid(InterfaceEvent.OnHoverEnd, OnHoverEnd);
        
        Events.RelayByVoid(InterfaceEvent.OnSpellSelected, OnSpellSelected);

        Events.RelayByVoid(GameEvent.OnSpellUsed, Shutdown);
        Events.RelayByVoid(GameEvent.OnTurnStart, Shutdown);
    }
    void Start() => infoPool = Repository.Get<GenericPool>(Pools.Info);
    
    void OnDestroy()
    {
        Events.BreakValueRelay<InfoAnchor,GameObject>(InterfaceEvent.OnHoverStart, OnHoverStart);
        Events.BreakVoidRelay(InterfaceEvent.OnHoverEnd, OnHoverEnd);
        
        Events.BreakVoidRelay(InterfaceEvent.OnSpellSelected, OnSpellSelected);

        Events.BreakVoidRelay(GameEvent.OnSpellUsed, Shutdown);
        Events.BreakVoidRelay(GameEvent.OnTurnStart, Shutdown);
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

    void OnSpellSelected()
    {
        HoverSignal.activeId = int.MaxValue;
        if (hasHoverInfo) OnHoverEnd();

        var map = Repository.Get<Map>(References.Map);
        foreach (var tileBase in map.Tiles.Values)
        {
            if (!(tileBase is Tile tile)) continue;

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
        HoverSignal.activeId = 0;

        foreach (var spellInfo in spellInfos) spellInfo.gameObject.SetActive(false);
        spellInfos.Clear();
    }
}