using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cross : Pattern
{
    [SerializeField] private Vector4Int directions;
    
    public override IEnumerable<Tile> GetTiles(Tile source) => source.GetCellsInCross(directions);
}