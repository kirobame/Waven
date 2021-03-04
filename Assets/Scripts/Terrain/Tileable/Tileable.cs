using System.Collections;
using Flux.Data;
using UnityEngine;

public abstract class Tileable : TileableBase, ITag
{
    [Space, SerializeField] private Animator animator;
    [SerializeField] private float speed;

    private Coroutine moveRoutine;

    public TeamTag Team
    {
        get => tag.Team;
        set => tag.Team = value;
    }
    private new Tag tag;
    
    //------------------------------------------------------------------------------------------------------------------/

    protected virtual void Awake() => tag = GetComponent<Tag>();
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public override void Place(Vector2 position) => transform.position = position;

    public override void Move(Vector2[] path)
    {
        IsMoving = true;
        moveRoutine = StartCoroutine(MoveRoutine(path));
    }
    protected virtual IEnumerator MoveRoutine(Vector2[] path)
    {
        var map = Repository.Get<Map>(References.Map);
        
        var index = 0;
        var time = 0.0f;

        SetOrientation((path[index + 1] - path[index]).ComputeOrientation());
        while (true)
        {
            time += Time.deltaTime;
            if (time >= speed)
            {
                navigator.SetCurrent(map.Tilemap.WorldToCell(path[index + 1]).ToTile());

                if (index + 1 >= path.Length - 1)
                {
                    transform.position = Vector2.Lerp(path[index], path[index + 1], 1.0f);
                    
                    EndMove();
                    OnMoveCompleted();
                    
                    moveRoutine = null;
                    yield break;
                }
                else
                {
                    time -= speed;
                    index++;

                    SetOrientation(( path[index + 1] - path[index]).ComputeOrientation());
                }
            }

            transform.position = Vector2.Lerp(path[index], path[index + 1], time / speed);
            yield return new WaitForEndOfFrame();
        }
    }
    protected virtual void OnMoveCompleted() { }

    public void InterruptMove()
    {
        if (moveRoutine == null) return;
        
        StopCoroutine(moveRoutine);
        moveRoutine = null;

        transform.position = navigator.Current.GetWorldPosition();
        
        EndMove();
        OnMoveCompleted();
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    protected void SetOrientation(Vector2Int value)
    {
        animator.SetFloat("X", value.x);
        animator.SetFloat("Y", value.y);
    }
}