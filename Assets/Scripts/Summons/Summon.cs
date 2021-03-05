using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Tileable, ILink
{
    public event Action<ILink> onDestroyed;
    
    public ITurnbound Owner { get; set; }
    
    //------------------------------------------------------------------------------------------------------------------/

    public virtual void Activate() { }
    public virtual void Deactivate() { }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onDestroyed?.Invoke(this);
    }

    //------------------------------------------------------------------------------------------------------------------/

    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false)
    {
        Owner.IncreaseBusiness();
        base.Move(path, speed, overrideSpeed);
    }
    protected override void OnMoveCompleted() => Owner.DecreaseBusiness();
}