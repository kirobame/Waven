using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Tileable, ILink
{
    public event Action<ILink> onDestroyed;

    public ITurnbound Owner
    {
        get => owner;
        set
        {
            if (hasOwner && value == null) hasOwner = false;
            else if (!hasOwner && value != null) hasOwner = true;

            owner = value;
        }
    }
    private ITurnbound owner;
    
    [SerializeField] float speed = 0.3f;

    private bool hasOwner;
    
    public void ChangeOwner()
    {
        if (this.TryGetComponent<Tag>(out var tag)) tag.Team = Player.Active.Team;
        Player.Active.AddDependency(this.gameObject);
    }

    public virtual void Activate() { }
    public virtual void Deactivate() { }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onDestroyed?.Invoke(this);
    }

    public void RockToGolem()
    {
        ChangeOwner();
        Activate();
    }
    public void GolemToRock()
    {
        Deactivate();
    }

    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false, bool processDir = true)
    {
        if (speed <= 0 || !overrideSpeed) speed = this.speed;

        if (hasOwner) Owner.IncreaseBusiness();
        base.Move(path, speed, overrideSpeed, processDir);
    }
    protected override void OnMoveCompleted()
    {
        if (!hasOwner) return;
        Owner.DecreaseBusiness();
    }
}
