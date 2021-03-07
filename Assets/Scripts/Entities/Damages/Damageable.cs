using Flux.Event;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Event;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public bool IsInvulnerable { get; private set; }

    public List<Life> Lives => lives;
    [SerializeField] private List<Life> lives;
    
    public TeamTag Team
    {
        get => tag.Team;
        set => tag.Team = value;
    }
    private new Tag tag;
    private new Player player;
    
    //------------------------------------------------------------------------------------------------------------------/

    void Awake()
    {
        Events.Open(GameEvent.OnDamageTaken);
        tag = GetComponent<Tag>();
        lives.Sort();
        player = gameObject.GetComponent<Player>();
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
        

        player.isTakingDamage = true;
        Events.ZipCall(GameEvent.OnDamageTaken, damage);

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
                    OnDeath();
                    return 2;
                }
            }
            else
            {
                Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
                return 3;
            }
        }
        player.isTakingDamage = false;

        Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
        return 3;
    }
    
    //------------------------------------------------------------------------------------------------------------------/
    
    protected virtual void OnDeath() => Destroy(gameObject);
}