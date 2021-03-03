using System;
using System.Collections;
using Flux;
using Flux.Data;
using Flux.Event;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Tileable, ITurnbound
{
    public event Action onFree;
    public event Action<Motive> onIntendedTurnStop;

    public string Name => name;

    public bool IsBusy => business > 0;
    
    public Match Match { get; set; }
    public short Initiative => initiative;
    
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    
    [Space, SerializeField] private short initiative;

    private InputAction spacebarAction;

    private ushort business;
    
    private IActivable[] activables;
    private bool isActive;
    
    //------------------------------------------------------------------------------------------------------------------/
    
    void Start()
    {
        SetOrientation(Vector2Int.right);
        activables = GetComponentsInChildren<IActivable>();
        
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        spacebarAction = inputs["Core/Spacebar"];
        spacebarAction.performed += OnSpacebarPressed;
    }
    void OnDestroy() => spacebarAction.performed -= OnSpacebarPressed;

    void OnSpacebarPressed(InputAction.CallbackContext context)
    {
        if (!isActive) return;
        
        Routines.Start(Routines.DoAfter(() =>
        {
            onIntendedTurnStop?.Invoke(new IntendedStopMotive());

        }, new YieldFrame()));
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    public void Activate()
    {
        isActive = true;
        foreach (var activable in activables) activable.Activate();
    }
    public void Interrupt(Motive motive)
    {
        isActive = false;
        foreach (var activable in activables) activable.Deactivate();
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    public override void Place(Vector2 position) => transform.position = position;
    public override void Move(Vector2[] path)
    {
        IsMoving = true;
        business++;
        
        StartCoroutine(MoveRoutine(path));
    }
    
    private IEnumerator MoveRoutine(Vector2[] path)
    {
        var index = 0;
        var time = 0.0f;

        ComputeOrientation();
        while (true)
        {
            time += Time.deltaTime;
            if (time >= speed)
            {
                if (index + 1 >= path.Length - 1)
                {
                    Execute(1.0f);
                    IsMoving = false;
                    
                    business--;
                    if (business == 0) onFree?.Invoke();
                    
                    yield break;
                }
                else
                {
                    time -= speed;
                    index++;

                    ComputeOrientation();
                }
            }

            Execute(time / speed);
            yield return new WaitForEndOfFrame();
        }

        void Execute(float ratio) => transform.position = Vector2.Lerp(path[index], path[index + 1], ratio);
        void ComputeOrientation()
        {
            var direction = path[index + 1] - path[index];
            
            if (direction.x < 0 && direction.y > 0) SetOrientation(Vector2Int.left);
            else if (direction.x > 0 && direction.y > 0) SetOrientation(Vector2Int.up);
            else if (direction.x > 0 && direction.y < 0) SetOrientation(Vector2Int.right);
            else SetOrientation(Vector2Int.down);
        }
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    private void SetOrientation(Vector2Int value)
    {
        animator.SetFloat("X", value.x);
        animator.SetFloat("Y", value.y);
    }
}
