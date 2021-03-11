using System.Collections;
using Flux.Data;
using Flux.Event;
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

public class Tileable : TileableBase, ITag
{
    public Vector2 LastDirection { get; private set; }
    
    public TeamTag Team
    {
        get => tag.Team;
        set => tag.Team = value;
    }
    private new Tag tag;

    private bool hasDamageable;
    private IDamageable damageable;

    private bool isPaused;
    private Coroutine moveRoutine;
    
    //------------------------------------------------------------------------------------------------------------------/

    protected virtual void Awake()
    {
        tag = GetComponent<Tag>();
        hasDamageable = TryGetComponent<IDamageable>(out damageable);
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    public override void Place(Vector2 position) => transform.position = position;

    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false, bool processDir = true)
    {
        if (speed <= 0)
        {
            Debug.LogError($"Trying to move : {this} with a negative speed!");
            return;
        }
        
        StartMove();
        moveRoutine = StartCoroutine(MoveRoutine(path, speed, processDir));
    }

    protected virtual IEnumerator MoveRoutine(Vector2[] path, float speed, bool processDir)
    {
        var map = Repository.Get<Map>(References.Map);
        
        var index = 0;
        var time = 0.0f;

        LastDirection = (path[index + 1] - path[index]).normalized;
        if (processDir) ProcessMoveDirection(LastDirection);
        
        while (true)
        {
            if (isPaused)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            
            time += Time.deltaTime;
            if (time >= speed)
            {
                navigator.SetCurrent(map.Tilemap.WorldToCell(path[index + 1]).ToTile());
                ProcessNewTile(navigator.Current);
                
                if (hasDamageable && index + 1 < path.Length - 1)
                {
                    var nextTile = map.Tilemap.WorldToCell(path[index + 2]).ToTile();
                    if (!nextTile.IsFree())
                    {
                        damageable.Inflict(1, DamageType.Base);
                        foreach (var entity in nextTile.Entities)
                        {
                            if (!entity.TryGet<IDamageable>(out damageable)) continue;
                            damageable.Inflict(1, DamageType.Base);
                        }
                        
                        if (!IsMoving) yield break;
                        LocalEnd();
                    
                        yield break;
                    }
                }

                if (index + 1 >= path.Length - 1)
                {
                    if (!IsMoving) yield break;
                    LocalEnd();
                    
                    yield break;
                }
                else
                {
                    time -= speed;
                    index++;

                    LastDirection = (path[index + 1] - path[index]).normalized;
                    if (processDir) ProcessMoveDirection(LastDirection);
                }
            }

            transform.position = Vector2.Lerp(path[index], path[index + 1], time / speed);
            yield return new WaitForEndOfFrame();
        }

        void LocalEnd()
        {
            transform.position = Vector2.Lerp(path[index], path[index + 1], 1.0f);
                    
            EndMove();
            OnMoveCompleted();
                    
            moveRoutine = null;
        }
    }
    
    protected virtual void OnMoveCompleted() { }
    protected virtual void ProcessNewTile(Tile tile) { }
    protected virtual void ProcessMoveDirection(Vector2 direction) { }

    public void PauseMove() => isPaused = true;
    public bool ResumeMove() => isPaused = false;
    
    public void InterruptMove()
    {
        if (moveRoutine == null) return;
        IsMoving = false;
        
        EndMove();
        OnMoveCompleted();
        
        transform.position = navigator.Current.GetWorldPosition();
        
        StopCoroutine(moveRoutine);
        moveRoutine = null;
    }
}