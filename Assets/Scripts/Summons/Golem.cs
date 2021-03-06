using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Tileable, ILink
{
    [SerializeField] float speed = 0.3f;
    public event Action<ILink> onDestroyed;
    public ITurnbound Owner { get; set; }
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

    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false)
    {
        if (speed <= 0 || !overrideSpeed) speed = this.speed;

        base.Move(path, speed, overrideSpeed);
        Owner.IncreaseBusiness();
    }
    protected override void OnMoveCompleted() => Owner.DecreaseBusiness();

}
