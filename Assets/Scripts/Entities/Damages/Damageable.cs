using System.Collections.Generic;
using System.Linq;
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
    
    //------------------------------------------------------------------------------------------------------------------/

    void Awake()
    {
        tag = GetComponent<Tag>();
        lives.Sort();
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    public void AddLife(Life value)
    {
        lives.Add(value);
        lives.Sort();
    }

    public int Inflict(int damage, DamageType type)
    {
        var index = 0;
        if (IsInvulnerable) return 0;
        
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
                    Destroy(gameObject);
                    return 2;
                }
            }
            else return 3;
        }
        
        return 3;
    }
}