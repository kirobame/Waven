using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDirectionalVfxGiver", menuName = "Waven/Vfx/Simple")]
public class SimpleVfxGiver : VfxGiver
{
    [SerializeField] private PoolableVfx prefab;

    public override PoolableVfx Get(EventArgs args) => prefab;
}