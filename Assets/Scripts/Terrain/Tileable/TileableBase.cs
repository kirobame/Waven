using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class TileableBase : MonoBehaviour, ITileable
{
    public event Action<ITileable> onMoveStart;
    public event Action<ITileable> onMoveDone;
    public event Action<ITileable> onDestroy;
    
    public Navigator Navigator => navigator;
    public bool IsMoving { get; protected set; }
    
    [SerializeField] protected Navigator navigator;

    protected virtual void OnDestroy()
    {
        onDestroy?.Invoke(this);
        
        if (IsMoving) EndMove();
        navigator.RemoveFromBoard();
    }
    
    public virtual void Place(Vector2 position) => transform.position = position;
    public virtual void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false, bool processDir = true) => throw new NotImplementedException();

    public virtual void SetOrientation(Vector2Int direction) { }
    
    protected void EndMove()
    {
        IsMoving = false;
        onMoveDone?.Invoke(this);
    }
    protected void StartMove()
    {
        IsMoving = true;
        onMoveStart?.Invoke(this);
    }
}