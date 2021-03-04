using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Life : IComparable<Life>
{
    public Life(string name, Color color, int actualValue, int maxValue, int priority, List<DamageType> handledTypes)
    {
        this.name = name;
        this.color = color;
        this.actualValue = actualValue;
        this.maxValue = maxValue; 
        this.priority = priority; 
        this.handledTypes = handledTypes;
    }
    
    public string name;
    public Color color;
    
    public int actualValue;
    public int maxValue;
    
    public int priority;

    public IReadOnlyList<DamageType> HandledTypes => handledTypes;
    public List<DamageType> handledTypes = new List<DamageType>();
    
    public int CompareTo(Life other) => priority.CompareTo(other.priority) * -1;
}