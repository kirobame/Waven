using UnityEngine;

public interface ITileable 
{
    Navigator Navigator { get; }
    bool IsMoving { get; }
    
    void Place(Vector2 position);
    void Move(Vector2[] path);
}