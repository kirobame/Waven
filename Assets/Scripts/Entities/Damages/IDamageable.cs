using System.Collections;
using System.Collections.Generic;

public interface IDamageable : ITag
{
    bool IsInvulnerable { get; }
    
    List<Life> Lives { get; }
    void AddLife(Life value);
    
    int Inflict(int damage, DamageType type);
}