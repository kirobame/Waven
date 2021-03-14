using System;

[Flags]
public enum CastTarget
{
    None = 0,
    
    Player = 1,
    Golem = 2,
    Trap = 4,
    Verglas = 8,
    Free = 16,
    
    Neutral = 32,
    Self = 64,
    Enemy = 128
}