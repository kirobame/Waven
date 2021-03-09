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
        
        Events.RelayByValue<InfoAnchor,Tileable>(InterfaceEvent.OnHoverStart, OnHoverStart);
        Events.RelayByVoid(InterfaceEvent.OnHoverEnd, OnHoverEnd);
        Events.RelayByValue<Spellcaster>(InterfaceEvent.OnSpellTilesAffect, OnSpellTilesAffect);
        Events.RelayByVoid(InterfaceEvent.OnSpellEnd, Shutdown);
    }
    void Start() => infoPool = Repository.Get<GenericPool>(Pools.Info);
    
    void OnDestroy()
    {
        Events.BreakValueRelay<InfoAnchor,Tileable>(InterfaceEvent.OnHoverStart, OnHoverStart);
        Events.BreakVoidRelay(InterfaceEvent.OnHoverEnd, OnHoverEnd);
        Events.BreakValueRelay<Spellcaster>(InterfaceEvent.OnSpellTilesAffect, OnSpellTilesAffect);
        Events.BreakVoidRelay(InterfaceEvent.OnSpellEnd, Shutdown);
    }

    private EntityInfo SetupInfoFor(InfoAnchor anchor, TileableBase source)
    {
        var instance = infoPool.CastSingle<EntityInfo>();
        
        instance.RectTransform.SetParent(transform);
        instance.AssignTo(anchor, source);

        return instance;
    }
    
    void OnHoverStart(InfoAnchor anchor, TileableBase source)
    {
        var check = spellInfos.Exists(info => info.Tile == source.Navigator.Current);
        if (check) return;
        
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
        var endHover = false;
        foreach (var tile in caster.AvailableTiles)
        {
            foreach (var entity in tile.Entities)
            {
                if (!(entity is TileableBase tileable)) return;

                var component = (Component)entity;
                if (!component.TryGet<InfoAnchor>(out var anchor)) continue;

                if (hasHoverInfo && !endHover && tileable.Navigator.Current == hoverInfo.Tile) endHover = true;
                spellInfos.Add(SetupInfoFor(anchor, tileable));
            }
        }
        
        if (endHover) OnHoverEnd();
    }
    
    void Shutdown()
    {
        foreach (var spellInfo in spellInfos) spellInfo.gameObject.SetActive(false);
        spellInfos.Clear();
        
        HoverSignal.activeId = 0;
        Events.EmptyCall(GameEvent.OnTileChange);
    }
}