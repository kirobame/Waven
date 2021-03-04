﻿using System;
using UnityEngine;

public class TileableBase : MonoBehaviour, ITileable
{
    public event Action onMoveDone;

    public Navigator Navigator => navigator;
    public bool IsMoving { get; protected set; }
    
    [SerializeField] protected Navigator navigator;

    protected virtual void OnDestroy() => navigator.RemoveFromBoard();
    
    public virtual void Place(Vector2 position) => transform.position = position;
    public virtual void Move(Vector2[] path) => throw new NotImplementedException();

    protected void EndMove()
    {
        IsMoving = false;
        onMoveDone?.Invoke();
    }
}