using System.Collections;
using UnityEngine;

public abstract class Tile
{
    public Tile(Vector2Int position, int height) 
    {
        this.position = position;
        this.height = height;
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    public Vector2Int FlatPosition => position;
    public int x => position.x;
    public int y => position.y;
    
    public Vector3Int Position => new Vector3Int(position.x, position.y, height);

    private Vector2Int position;
    private int height;
}
