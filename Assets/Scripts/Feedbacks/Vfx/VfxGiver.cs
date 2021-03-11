using System;
using UnityEngine;

public abstract class VfxGiver : ScriptableObject
{
    public VfxKey Key => key;
    [SerializeField] private VfxKey key;
     
    public abstract PoolableVfx Get(EventArgs args);
}