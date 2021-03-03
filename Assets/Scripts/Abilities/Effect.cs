using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using Flux.Editor;
using UnityEngine;

public abstract class Effect
{
    public abstract void PlayEffect(List<Vector3Int> pos);
}

[Serializable]
public class BasicDamage : Effect
{
    [SerializeField] uint range;
    [SerializeField] int damages;
    public override void PlayEffect(List<Vector3Int> pos)
    {
        Debug.Log("Deal so much fire damages");
    }
}