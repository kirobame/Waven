using System.Collections;
using System.Collections.Generic;

public interface IDamageable : ITag
{
    bool IsInvulnerable { get; }
    
    List<Life> Lives { get; }
    
    Life Get(string name);
    void AddLife(Life value);
    
    int Inflict(int damage, DamageType type);
}