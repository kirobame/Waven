using System;
using Flux.Event;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Event;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public event Action<IDamageable> onFeedbackDone;
    
    public bool IsInvulnerable { get; private set; }

    public List<Life> Lives => lives;
    [SerializeField] private List<Life> lives= new List<Life>()
    {
        new Life("Health", new Color(0.2812389f, 0.754717f, 0.303109f, 1.0f), 10, 10, 0, new List<DamageType>() { DamageType.Base })
    };
    
    public TeamTag Team
    {
        get => tag.Team;
        set => tag.Team = value;
    }
    private new Tag tag;
    
    //------------------------------------------------------------------------------------------------------------------/

    protected virtual void Awake()
    {
        tag = GetComponent<Tag>();
        lives.Sort();
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    public Life Get(string name) => lives.First(life => life.name.ToLower().Equals(name.ToLower()));
    public void AddLife(Life value)
    {
        lives.Add(value);
        lives.Sort();
    }

    public int Inflict(int damage, DamageType type)
    {
        var index = 0;
        
        if (IsInvulnerable) return 0;
        if (!lives.Any()) return 2;

        OnDamageTaken();

        while (damage > 0)
        {
            if (!lives[index].HandledTypes.Contains(type)) return 1;

            lives[index].actualValue -= damage;
            if (lives[index].actualValue <= 0)
            {
                damage = -lives[index].actualValue;
                lives.RemoveAt(index);

                if (!lives.Any())
                {
                    EndFeedback();
                    OnDeath();

                    return 2;
                }
            }
            else
            {
                Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
                OnLogicDone();
                
                return 3;
            }
        }

        Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
        OnLogicDone();
        
        return 3;
    }
    
    //------------------------------------------------------------------------------------------------------------------/
    
    protected virtual void OnDamageTaken() { }

    protected virtual void OnLogicDone() => EndFeedback();
    protected void EndFeedback() => onFeedbackDone?.Invoke(this);
    
    protected virtual void OnDeath() => Destroy(gameObject);
}