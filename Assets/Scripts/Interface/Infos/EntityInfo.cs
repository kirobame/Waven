using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfo : MonoBehaviour
{
    public RectTransform RectTransform => (RectTransform)transform;
    
    [SerializeField] private Slider slider;
    [SerializeField] private StatInfo[] infos;

    [Space, SerializeField] private Color normal;
    [SerializeField] private Color bonus;
    [SerializeField] private Color malus;
    
    private bool isActive;
    private InfoAnchor anchor;
    private GameObject source;

    void OnEnable() => Events.RelayByVoid(InterfaceEvent.OnInfoRefresh, Refresh);
    void OnDisable()
    {
        if (isActive) anchor.onDestroyed -= OnAnchorDestroyed;
        
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

        anchor.onDestroyed += OnAnchorDestroyed;
        this.anchor = anchor;
        Place();

        this.source = source;
        Refresh();
    }
    
    private void Refresh()
    {
        if (!isActive) return;
        
        if (source == null)
        {
            gameObject.SetActive(false);
            return;
        }

        var index = 0;
        if (!source.TryGet<IDamageable>(out var damageable))
        {
            ClearFrom(index);
            return;
        }

        var stats = Repository.Get<Stats>(References.Stats);

        var health = damageable.Get("Health");
        if (health == null)
        {
            gameObject.SetActive(false);
            return;
        }
        var ratio = health.Ratio;

        slider.value = ratio;
        infos[index].TextMesh.color = normal;
        infos[index].Assign(stats.Values[StatType.Health], $"{health.actualValue}");
        
        index++;

        var hasAttributes = false;
        if (source.TryGet<IAttributeHolder>(out var attributes))
        {
            hasAttributes = true;
            
            if (HandleStat(index, attributes, new Id('D', 'M', 'G'), stats.Values[StatType.Damage])) index++;
            if (HandleStat(index, attributes, new Id('P', 'S', 'H'), stats.Values[StatType.Force])) index++;
        }
        
        if (source.TryGet<Moveable>(out var moveable))
        {
            moveable.Dirty();
            var move = moveable.PM;

            if (hasAttributes && attributes.Args.TryGetValue(new Id('M', 'V', 'T'), out var baseValue, out var value))
            {
                if (value == 0) infos[index].TextMesh.color = normal;
                else if (value > 0) infos[index].TextMesh.color = bonus;
                else infos[index].TextMesh.color = malus;
            }
            
            infos[index].Assign(stats.Values[StatType.Movement], $"{move}");
            index++;
        }

        ClearFrom(index);
    }
    private void ClearFrom(int index) { for (var i = index; i < infos.Length; i++) infos[i].Clear(); }

    private bool HandleStat(int index, IAttributeHolder attributes, Id id, Stats.Info info)
    {
        if (attributes.Args.TryGetValue(id, out var baseValue, out var value))
        {
            if (value == 0) infos[index].TextMesh.color = normal;
            else if (value > 0) infos[index].TextMesh.color = bonus;
            else infos[index].TextMesh.color = malus;
            
            infos[index].Assign(info, $"{baseValue + value}");
            
            return true;
        }

        return false;
    }
    private void Place()
    {
        var camera = Repository.Get<Camera>(References.Camera);

        var position = camera.WorldToScreenPoint(anchor.Position);
        RectTransform.position = position;
    }

    void OnAnchorDestroyed()
    {
        anchor.onDestroyed -= OnAnchorDestroyed;
        
        if (HoverSignal.activeId != 0 && HoverSignal.activeId != int.MaxValue)
        {
            HoverSignal.activeId = int.MaxValue;
            Events.EmptyCall(InterfaceEvent.OnHoverEnd);
        }
        
        isActive = false;
        gameObject.SetActive(false);
    }
}