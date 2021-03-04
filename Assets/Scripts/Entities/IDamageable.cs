using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DamageType
{
    Base
}

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

public interface IDamageable
{
    bool IsInvulnerable { get; }
    
    List<Life> Lives { get; }
    void AddLife(Life value);
    
    int TakeDamage(int damage, DamageType type);
}

public abstract class Damageable : MonoBehaviour, IDamageable, ITag
{
    private void Awake() => tag = GetComponent<Tag>();

    public bool IsInvulnerable { get; private set; }

    public List<Life> Lives => lives;
    [SerializeField] private List<Life> lives;
    
    public TeamTag Value => tag.Value;
    [SerializeField] private new Tag tag;

    public void AddLife(Life value)
    {
        lives.Add(value);
        lives.Sort();
    }
    public void AddLifeValue(string name, Color color, int actualValue, int maxValue, int priority, List<DamageType> handeledTypes)
    {
        lives.Add(new Life (name, color, actualValue, maxValue, priority, handeledTypes) );
        lives.Sort();
    }
    public void AddLifeValue(string name, Color color, int maxValue, int priority, List<DamageType> handeledTypes)
    {
        lives.Add(new Life(name, color, maxValue, maxValue, priority, handeledTypes));
        lives.Sort();
    }
    public int TakeDamage(int damage, DamageType type)
    {
        if (!IsInvulnerable) return 0;

        for (int i = 0; i < Lives.Count; i++)
        {
            if (!Lives[i].HandledTypes.Contains(type)) continue;

            Lives[i].actualValue -= damage;
            if (Lives[i].actualValue < 0)
            {
                if (i == Lives.Count - 1)
                {
                    //DEAD
                }
                Lives.RemoveAt(i);
            }
            return Lives[i].actualValue;
        }

        return 1;
    }
    public int TakeDamage(int damage)
    {
        if (!IsInvulnerable) return 0;

        for (int i = 0; i < Lives.Count; i++)
        {
            Lives[i].actualValue -= damage;
            if (Lives[i].actualValue < 0)
            {
                if (i == Lives.Count - 1)
                {
                    //DEAD
                }
                Lives.RemoveAt(i);
            }
            return Lives[i].actualValue;
        }

        return 1;
    }
    public int TakeDirectDamage(int damage)
    {
        Lives[Lives.Count-1].actualValue -= damage;
        if (Lives[Lives.Count - 1].actualValue < 0)
        {
            if (Lives.Count - 1 == Lives.Count - 1)
            {
                //DEAD
            }
            Lives.RemoveAt(Lives.Count - 1);
        }
        return Lives[Lives.Count - 1].actualValue;
    }
}
