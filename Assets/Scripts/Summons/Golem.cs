using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Tileable, ILink
{
    public event Action<ILink> onDestroyed;
    public ITurnbound Owner { get; set; }
    public void ChangeOwner()
    {

    }

    public void Activate()
    {
        throw new NotImplementedException();
    }

    public void Deactivate()
    {
        throw new NotImplementedException();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        onDestroyed?.Invoke(this);
    }

    public void RockToGolem()
    {
        Activate();
    }
    public void GolemToRock()
    {
        Deactivate();
    }

    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false)
    {
        Owner.IncreaseBusiness();
        base.Move(path, speed, overrideSpeed);
    }
    protected override void OnMoveCompleted() => Owner.DecreaseBusiness();

}
