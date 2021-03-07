using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class TileableBase : SerializedMonoBehaviour, ITileable
{
    public event Action onMoveDone;
    public event Action onMoveStart;
    public event Action<ITileable> onDestroy;
    public Navigator Navigator => navigator;
    public bool IsMoving { get; protected set; }
    
    [SerializeField] protected Navigator navigator;

    protected virtual void OnDestroy()
    {
        navigator.RemoveFromBoard();
        onDestroy?.Invoke(this);
    }
    
    public virtual void Place(Vector2 position) => transform.position = position;
    public virtual void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false) => throw new NotImplementedException();

    protected void EndMove()
    {
        IsMoving = false;
        onMoveDone?.Invoke();
    }
    protected void StartMove()
    {
        IsMoving = true;
        onMoveStart?.Invoke();
    }
}