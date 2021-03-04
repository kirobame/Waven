using System.Collections;
using UnityEngine;

public interface ITileable 
{
    Navigator Navigator { get; }
    bool IsMoving { get; }
    
    void Place(Vector2 position);
    void Move(Vector2[] path);
}

public abstract class Tileable : MonoBehaviour, ITileable
{
    public Navigator Navigator => navigator;
    public bool IsMoving { get; protected set; }

    [SerializeField] protected Navigator navigator;
    
    [Space, SerializeField] private Animator animator;
    [SerializeField] private float speed;
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public virtual void Place(Vector2 position) => transform.position = position;
    
    //------------------------------------------------------------------------------------------------------------------/
    
    public virtual void Move(Vector2[] path)
    {
        IsMoving = true;
        StartCoroutine(MoveRoutine(path));
    }
    protected virtual IEnumerator MoveRoutine(Vector2[] path)
    {
        var index = 0;
        var time = 0.0f;

        SetOrientation(( path[index + 1] - path[index]).ComputeOrientation());
        while (true)
        {
            time += Time.deltaTime;
            if (time >= speed)
            {
                if (index + 1 >= path.Length - 1)
                {
                    transform.position = Vector2.Lerp(path[index], path[index + 1], 1.0f);
                    IsMoving = false;
                    
                    OnMoveCompleted();

                    Inputs.isLocked = false;
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
    
    //------------------------------------------------------------------------------------------------------------------/

    protected void SetOrientation(Vector2Int value)
    {
        animator.SetFloat("X", value.x);
        animator.SetFloat("Y", value.y);
    }
}
