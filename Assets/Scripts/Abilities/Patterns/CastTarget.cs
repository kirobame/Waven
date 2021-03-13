using System;

[Flags]
public enum CastTarget
{
    None = 0,
    
    Player = 1,
    Golem = 2,
    Trap = 4,
    Free = 8,
    
    Neutral = 16,
    Self = 32,
    Enemy = 64
}