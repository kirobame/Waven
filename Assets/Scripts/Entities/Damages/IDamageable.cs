using System;
using System.Collections;
using System.Collections.Generic;

public interface IDamageable : ITag
{
    event Action<IDamageable> onFeedbackDone;
    
    bool IsAlive { get; }
    bool IsInvulnerable { get; set; }
    
    List<Life> Lives { get; }
    
    Life Get(string name);
    void AddLife(Life value);
    
    int Inflict(int damage, DamageType type);
}