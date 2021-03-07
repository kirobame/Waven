using System;
using System.Collections.Generic;
using Flux.Data;
using UnityEngine;

public class Stats : MonoBehaviour
{
    #region Nested Types

    [Serializable]
    public class Info
    {
        public StatType Type => type;
        [SerializeField] private StatType type;
        
        public Sprite Icon => icon;
        [SerializeField] private Sprite icon;

        public Color Color => color;
        [SerializeField] private Color color;
    }
    
    #endregion

    public IReadOnlyDictionary<StatType, Info> Values => values;
    private Dictionary<StatType, Info> values;
    
    [SerializeField] private Info[] infos;

    void Awake()
    {
        values = new Dictionary<StatType, Info>();
        foreach (var info in infos)
        {
            if (values.ContainsKey(info.Type)) continue;
            values.Add(info.Type, info);
        }
        
        Repository.Register(References.Stats, this);
    }
    void OnDestroy() => Repository.Unregister(References.Stats);
}