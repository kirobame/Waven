using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Tile(Vector2Int pos, int height) 
    {
        position = pos;
        this.height = height;
    }

    public Vector3Int Position => new Vector3Int(position.x, position.y, height);

    private Vector2Int position;
    private int height;
}
