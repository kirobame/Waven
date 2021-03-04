using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Tileable, ILink
{
    public event Action<ILink> onDestroyed;
    
    public ITurnbound Owner { get; set; }

    [SerializeField] private int lifetime;
    private int countdown;

    protected override void Awake()
    {
        base.Awake();
        countdown = lifetime;
    }

    public void Activate()
    {
        /*countdown--;
        if (countdown <= 0) Destroy(gameObject);*/
    }
    public void Deactivate() { }

    void OnDestroy() => onDestroyed?.Invoke(this);

    public override void Move(Vector2[] path)
    {
        Owner.IncreaseBusiness();
        base.Move(path);
    }
    protected override void OnMoveCompleted() => Owner.DecreaseBusiness();
}
