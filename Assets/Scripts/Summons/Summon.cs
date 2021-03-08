using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : Tileable, ILink
{
    public event Action<ILink> onDestroyed;
    [SerializeField] float speed = 0.3f;
    
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

    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false, bool processDir = true)
    {
        if (speed <= 0 || !overrideSpeed) speed = this.speed;
        
        Owner.IncreaseBusiness();
        base.Move(path, speed, overrideSpeed, processDir);
    }
    protected override void OnMoveCompleted() => Owner.DecreaseBusiness();
}