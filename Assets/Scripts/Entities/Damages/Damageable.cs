using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable, ITag
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

    void Awake() => tag = GetComponent<Tag>();

    //------------------------------------------------------------------------------------------------------------------/
    
    public void AddLife(Life value)
    {
        lives.Add(value);
        lives.Sort();
    }

    public int Inflict(int damage, DamageType type)
    {
        if (IsInvulnerable) return 0;

        for (int i = 0; i < Lives.Count; i++)
        {
            if (!Lives[i].HandledTypes.Contains(type)) continue;

            Lives[i].actualValue -= damage;
            if (Lives[i].actualValue < 0)
            {
                if (i == Lives.Count - 1)
                {
                    Destroy(gameObject);
                    return 2;
                }
                Lives.RemoveAt(i);
            }
            return Lives[i].actualValue;
        }

        return 1;
    }
}