﻿using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfo : MonoBehaviour
{
    public RectTransform RectTransform => (RectTransform)transform;
    
    [SerializeField] private Slider slider;
    [SerializeField] private StatInfo[] infos;

    private bool isActive;
    private InfoAnchor anchor;
    private GameObject source;

    void OnEnable() => Events.RelayByVoid(InterfaceEvent.OnInfoRefresh, Refresh);
    void OnDisable()
    {
        isActive = false;
        Events.BreakVoidRelay(InterfaceEvent.OnInfoRefresh, Refresh);
    }

    void Update()
    {
        if (!isActive) return;
        Place();
    }

    public void AssignTo(InfoAnchor anchor, GameObject source)
    {
        isActive = true;

        this.anchor = anchor;
        Place();

        this.source = source;
        Refresh();
    }
    
    private void Refresh()
    {
        if (!isActive) return;

        var index = 0;
        if (!source.TryGet<IDamageable>(out var damageable))
        {
            ClearFrom(index);
            return;
        }

        var stats = Repository.Get<Stats>(References.Stats);

        var health = damageable.Get("Health");
        var ratio = health.Ratio;

        slider.value = ratio;
        infos[index].Assign(stats.Values[StatType.Health], $"{health.actualValue} / {health.maxValue}");
        
        index++;

        var hasAttributes = false;
        if (source.TryGet<IAttributeHolder>(out var attributes))
        {
            hasAttributes = true;
            
            var damage = 1;
            if (attributes.Args.TryGet<IWrapper<int>>(new Id('D', 'M', 'G'), out var damageArgs)) damage += damageArgs.Value;
            
            infos[index].Assign(stats.Values[StatType.Damage], $"{damage}");
            index++;
            
            var push = 1;
            if (attributes.Args.TryGet<IWrapper<int>>(new Id('P', 'S', 'H'), out var pushArgs)) push += damageArgs.Value;
            
            infos[index].Assign(stats.Values[StatType.Force], $"{push}");
            index++;
        }
        
        if (source.TryGet<Moveable>(out var moveable))
        {
            var move = moveable.PM;
            if (hasAttributes && attributes.Args.TryGet<IWrapper<int>>(new Id('M', 'V', 'T'), out var moveArgs)) move += moveArgs.Value;
                
            infos[index].Assign(stats.Values[StatType.Movement], $"{move}");
            index++;
        }

        ClearFrom(index);
    }
    private void ClearFrom(int index) { for (var i = index; i < infos.Length; i++) infos[i].Clear(); }

    private void Place()
    {
        var camera = Repository.Get<Camera>(References.Camera);

        var position = camera.WorldToScreenPoint(anchor.Position);
        RectTransform.position = position;
    }
}