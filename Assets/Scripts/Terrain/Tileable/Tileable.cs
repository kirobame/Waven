using System.Collections;
using Flux.Data;
using UnityEngine;

public class Tileable : TileableBase, ITag
{
    public TeamTag Team
    {
        get => tag.Team;
        set => tag.Team = value;
    }
    private new Tag tag;
    
    private Coroutine moveRoutine;
    
    //------------------------------------------------------------------------------------------------------------------/

    protected virtual void Awake() => tag = GetComponent<Tag>();
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public override void Place(Vector2 position) => transform.position = position;

    public override void Move(Vector2[] path, float speed = -1.0f, bool overrideSpeed = false)
    {
        if (speed <= 0)
        {
            Debug.LogError($"Trying to move : {this} with a negative speed!");
            return;
        }
        StartMove();
        moveRoutine = StartCoroutine(MoveRoutine(path, speed));
    }
    protected virtual IEnumerator MoveRoutine(Vector2[] path, float speed)
    {
        var map = Repository.Get<Map>(References.Map);
        
        var index = 0;
        var time = 0.0f;

        ProcessMoveDirection(path[index + 1] - path[index]);
        while (true)
        {
            time += Time.deltaTime;
            if (time >= speed)
            {
                navigator.SetCurrent(map.Tilemap.WorldToCell(path[index + 1]).ToTile());

                if (index + 1 >= path.Length - 1)
                {
                    if (!IsMoving) yield break;

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

                    ProcessMoveDirection(path[index + 1] - path[index]);
                }
            }

            transform.position = Vector2.Lerp(path[index], path[index + 1], time / speed);
            yield return new WaitForEndOfFrame();
        }
    }
    protected virtual void OnMoveCompleted() { }

    protected virtual void ProcessMoveDirection(Vector2 direction) { }
    
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