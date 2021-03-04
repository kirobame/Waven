﻿using System.Collections;
using System.Collections.Generic;

public interface IDamageable
{
    bool IsInvulnerable { get; }
    
    List<Life> Lives { get; }
    void AddLife(Life value);
    
    int Inflict(int damage, DamageType type);
}