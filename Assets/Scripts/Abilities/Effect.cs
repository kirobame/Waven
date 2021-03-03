using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using Flux.Editor;
using UnityEngine;

public abstract class Effect
{
    [SerializeField] object pattern;
    [SerializeField] bool needNewSelection;
    public bool NeedNewSelection() { return needNewSelection; }

    public abstract void LaunchEffect(Tile selectedTile);
    public abstract List<Tile> GetAffectedTiles(Tile selectedTile);
    public abstract void PlayEffect(List<Tile> affectedTiles);
}

[Serializable]
public class BasicDamage : Effect
{

    [SerializeField] uint range;
    [SerializeField] int damages;

    public override void LaunchEffect(Tile selectedTile)
    {
        PlayEffect(GetAffectedTiles(selectedTile));
    }
    public override List<Tile> GetAffectedTiles(Tile selectedTile)
    {
        List<Tile> affectedTiles = new List<Tile>(); //Get tiles en fonction du pattern

        return affectedTiles;
    }
    public override void PlayEffect(List<Tile> affectedTiles)
    {
        Debug.Log("Deal so much fire damages");
    }


}