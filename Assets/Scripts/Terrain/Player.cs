using UnityEngine;

public class Player : Movable
{
    public override void Move(Vector2[] path)
    {
        //throw new System.NotImplementedException();
    }

    public override void Place(Vector2 position)
    {
        transform.position = position + Vector2.up*0.325f;
    }
}
