using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes
{
    Base
}

public class damageablePoints
{
    damageablePoints (name, color, actualValue, maxValue, priority, handeledTypes)
    string name;
    Color color;
    int actualValue;
    int maxValue;
    int priority;
    List<DamageTypes> handeledTypes = new List<DamageTypes>();
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
    }
    public void AddLifeValue(string name, Color color, int actualValue, int maxValue, int priority, List<DamageTypes> handeledTypes)
    {
        lifeValues.Add(new damageablePoints (name, color, actualValue, maxValue, priority, handeledTypes) );
    }

    public int TakeDamage(int damage, DamageTypes type)
    {
        if (!damageable) return 0;

        for (int i = lifeValues.Count - 1; i < lifeValues.Count; i--)
        {

        }

        return 1;
    }
    public int TakeDamage(int damage)
    {
    }
}
