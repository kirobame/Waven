using System;
using System.Collections;
using System.Collections.Generic;
using Flux.Data;
using Flux.Event;
using Flux.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Banner : MonoBehaviour
{
    [SerializeField] private Sequencer entry;
    [SerializeField] private Sequencer exit;
    [SerializeField] private float waitTime;

    [Space, SerializeField] private TMP_Text titleMesh;
    [SerializeField] private TMP_Text challengeMesh;
    [SerializeField] private TMP_Text textMesh;
    [SerializeField] private Image icon;

    [Space, SerializeField] private TMP_FontAsset redFont;
    [SerializeField] private TMP_FontAsset blueFont;

    private InputAction clickAction;
    
    private SendbackArgs entryArgs;
    private SendbackArgs exitArgs;

    private Challenge current;
    private Coroutine waitRoutine;

    void Awake()
    {
        entryArgs = new SendbackArgs();
        entryArgs.onDone += OnEntryDone;
        
        exitArgs = new SendbackArgs();
        exitArgs.onDone += OnExitDone;
        
        Events.Open(InterfaceEvent.OnChallengeUpdate);
        Events.Open(InterfaceEvent.OnChallengeCompleted);
        Events.Open(InterfaceEvent.OnChallengeFailed);
        
        Events.RelayByValue<Challenge>(InterfaceEvent.OnChallengeUpdate, Display);
        Events.RelayByVoid(GameEvent.OnRewardStart, OnRewardStart);
    }
    void Start()
    {
        var inputs = Repository.Get<InputActionAsset>(References.Inputs);
        clickAction = inputs["Core/Click"];
    }
    
    void OnDestroy()
    {
        if (waitRoutine != null)
        {
            clickAction.performed -= OnClick;
            waitRoutine = null;
        }
        
        Events.BreakValueRelay<Challenge>(InterfaceEvent.OnChallengeUpdate, Display);
        Events.BreakVoidRelay(GameEvent.OnRewardStart, OnRewardStart);
    }

    void OnRewardStart()
    {
        if (waitRoutine != null)
        {
            StopCoroutine(waitRoutine);
            End(EventArgs.Empty);
        }
    }
    
    public void Display(Challenge challenge)
    {
        this.current = challenge;

        if (waitRoutine != null)
        {
            StopCoroutine(waitRoutine);
            End(exitArgs);
        }
        else Display();
    }
    private void Display()
    {
        titleMesh.text = $"Au tour du joueur 0{Player.Active.Index + 1} !";
        titleMesh.font = Player.Active.Index == 0 ? blueFont : redFont;
        
        challengeMesh.text = $"Challenge : {current.Title}";
        textMesh.text = current.GetDescription();
        icon.sprite = current.Icon;
        
        entry.Play(entryArgs);
    }

    void OnEntryDone(EventArgs args)
    {
        clickAction.performed += OnClick;
        waitRoutine = StartCoroutine(WaitRoutine());
    }
    void OnExitDone(EventArgs ars)
    {
        Display(current);
    }

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(waitTime);
        End(EventArgs.Empty);
    }
    private void End(EventArgs args)
    {
        clickAction.performed -= OnClick;
        
        waitRoutine = null;
        exit.Play(args);
    }

    void OnClick(InputAction.CallbackContext context)
    {
        StopCoroutine(waitRoutine);
        End(EventArgs.Empty);
    }
}
