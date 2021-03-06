using System;
using UnityEngine;

public class TileableBase : MonoBehaviour, ITileable
{
    public event Action<ITileable> onMoveDone;

    public Navigator Navigator => navigator;
    public bool IsMoving { get; protected set; }
    
    [SerializeField] protected Navigator navigator;

    protected virtual void OnDestroy()
    {
        if (IsMoving) EndMove();
        navigator.RemoveFromBoard();
    }

    public virtual void Place(Vector2 position) => transform.position = position;
    public virtual void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false) => throw new NotImplementedException();

    protected void EndMove()
    {
        Debug.Log($"ENDING MOVE FOR : {this}");
        
        IsMoving = false;
        onMoveDone?.Invoke(this);
    }
}