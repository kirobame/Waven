using UnityEngine;

public interface ITileable 
{
    bool IsMoving { get; }
    
    void Place(Vector2 position);
    void Move(Vector2[] path);
}

public abstract class Tileable : MonoBehaviour, ITileable
{
    public bool IsMoving { get; protected set; }
    
    public abstract void Place(Vector2 position);
    public abstract void Move(Vector2[] path);
}
