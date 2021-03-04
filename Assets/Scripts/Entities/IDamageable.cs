using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes
{
    Base
}

public class damageablePoints : IComparer<damageablePoints>
{
    public damageablePoints(string _name, Color _color, int _actualValue, int _maxValue, int _priority, List<DamageTypes> _handeledTypes)
    { name = _name; color = _color; actualValue = _actualValue; maxValue = _maxValue; priority = _priority; handeledTypes = _handeledTypes; }
    public string name;
    public Color color;
    public int actualValue;
    public int maxValue;
    public int priority;
    public List<DamageTypes> handeledTypes = new List<DamageTypes>();

    public int Compare(damageablePoints a, damageablePoints b)
    {
        if (a.priority > b.priority)
            return 1;
        else if (a.priority < b.priority)
            return -1;
        else
            return 0;
    }
}

public interface IDamageable
{
    bool damageable { get; }
    List<damageablePoints> lifeValues { get; }
    void AddLifeValue(damageablePoints value);
    int TakeDamage(int damage, DamageTypes type);
}

public abstract class Damageable : MonoBehaviour, IDamageable
{
    public bool damageable { get; private set; }
    public List<damageablePoints> lifeValues { get; private set; }

    public void AddLifeValue(damageablePoints value)
    {
        lifeValues.Add(value);
        lifeValues.Sort();
    }
    public void AddLifeValue(string name, Color color, int actualValue, int maxValue, int priority, List<DamageTypes> handeledTypes)
    {
        lifeValues.Add(new damageablePoints (name, color, actualValue, maxValue, priority, handeledTypes) );
        lifeValues.Sort();
    }
    public void AddLifeValue(string name, Color color, int maxValue, int priority, List<DamageTypes> handeledTypes)
    {
        lifeValues.Add(new damageablePoints(name, color, maxValue, maxValue, priority, handeledTypes));
        lifeValues.Sort();
    }
    public int TakeDamage(int damage, DamageTypes type)
    {
        if (!damageable) return 0;

        for (int i = 0; i < lifeValues.Count; i++)
        {
            if (!lifeValues[i].handeledTypes.Contains(type)) continue;

            lifeValues[i].actualValue -= damage;
            if (lifeValues[i].actualValue < 0)
            {
                if (i == lifeValues.Count - 1)
                {
                    //DEAD
                }
                lifeValues.RemoveAt(i);
            }
            return lifeValues[i].actualValue;
        }

        return 1;
    }
    public int TakeDamage(int damage)
    {
        if (!damageable) return 0;

        for (int i = 0; i < lifeValues.Count; i++)
        {
            lifeValues[i].actualValue -= damage;
            if (lifeValues[i].actualValue < 0)
            {
                if (i == lifeValues.Count - 1)
                {
                    //DEAD
                }
                lifeValues.RemoveAt(i);
            }
            return lifeValues[i].actualValue;
        }

        return 1;
    }
    public int TakeDirectDamage(int damage)
    {
        lifeValues[lifeValues.Count-1].actualValue -= damage;
        if (lifeValues[lifeValues.Count - 1].actualValue < 0)
        {
            if (lifeValues.Count - 1 == lifeValues.Count - 1)
            {
                //DEAD
            }
            lifeValues.RemoveAt(lifeValues.Count - 1);
        }
        return lifeValues[lifeValues.Count - 1].actualValue;
    }
}
