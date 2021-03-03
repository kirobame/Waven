using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using Flux.Editor;
using UnityEngine;

public abstract class Effect
{
    [SerializeField] protected Pattern pattern;
    [SerializeField] protected bool needNewSelection;
    public bool NeedNewSelection() { return needNewSelection; }

    public abstract void LaunchEffect(WalkableTile selectedTile);
    public abstract List<WalkableTile> GetAffectedTiles(WalkableTile selectedTile);
    public abstract void PlayEffect(List<WalkableTile> affectedTiles);
}

[Serializable]
public class BasicDamage : Effect
{
    [SerializeField] uint range;
    [SerializeField] int damages;

    public override void LaunchEffect(WalkableTile selectedTile)
    {
        PlayEffect(GetAffectedTiles(selectedTile));
    }
    public override List<WalkableTile> GetAffectedTiles(WalkableTile selectedTile)
    {
        return pattern.GetTiles(selectedTile);
    }
    public override void PlayEffect(List<WalkableTile> affectedTiles)
    {
        Debug.Log("Deal so much fire damages");
    }


}