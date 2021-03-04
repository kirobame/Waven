using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Circle : Pattern
{
    [SerializeField] private int radius;
    
    public override IEnumerable<Tile> GetTiles(Tile source) => source.GetCellsAround(radius);
}