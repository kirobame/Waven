using System;
using UnityEngine;

public interface ITileable
{
    event Action<ITileable> onMoveDone;
    
    Navigator Navigator { get; }
    bool IsMoving { get; }
    
    void Place(Vector2 position);
    void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false, bool processDir = true);

    void SetOrientation(Vector2Int direction);
}