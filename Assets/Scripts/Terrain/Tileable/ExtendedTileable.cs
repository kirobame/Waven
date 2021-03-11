using UnityEngine;

public class ExtendedTileable : Tileable
{
    public Animator Animator => animator;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector2Int orientation;

    [Space, SerializeField] private float speed;
    
    protected virtual void Start() => SetOrientation(orientation);
    
    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false, bool processDir = true)
    {
        if (speed <= 0 || !overrideSpeed) speed = this.speed;
        base.Move(path, speed, overrideSpeed, processDir);

        if (processDir) animator.SetBool("isMoving", true);
    }

    protected override void ProcessMoveDirection(Vector2 direction)
    {
        var orientation = direction.ComputeOrientation();
        SetOrientation(orientation);
    }
    public override void SetOrientation(Vector2Int direction)
    {
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }

    protected override void OnMoveCompleted() => animator.SetBool("isMoving", false);
}