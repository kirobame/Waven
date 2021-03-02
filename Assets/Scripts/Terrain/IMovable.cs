using UnityEngine;

public interface IMovable 
{
    void Place(Vector2 position);
    void Move(Vector2[] path);
}

public abstract class Movable : MonoBehaviour, IMovable
{
    public abstract void Move(Vector2[] path);
    public abstract void Place(Vector2 position);
}
