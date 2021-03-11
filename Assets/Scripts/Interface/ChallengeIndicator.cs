﻿using Flux.Event;
using Flux.Feedbacks;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChallengeIndicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform position;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject complete;
    [SerializeField] private GameObject fail;

    [SerializeField] private Sequencer challengeDone;
    [SerializeField] private Sequencer challengeFailed;

    private Challenge current;
    
    void Awake()
    {
        Events.RelayByValue<Challenge>(InterfaceEvent.OnChallengeUpdate, Refresh);
        Events.RelayByVoid(InterfaceEvent.OnChallengeCompleted, OnCompletion);
        Events.RelayByVoid(InterfaceEvent.OnChallengeFailed, OnFail);
    }
    void OnDestroy()
    {
        Events.BreakValueRelay<Challenge>(InterfaceEvent.OnChallengeUpdate, Refresh);
        Events.BreakVoidRelay(InterfaceEvent.OnChallengeCompleted, OnCompletion);
        Events.BreakVoidRelay(InterfaceEvent.OnChallengeFailed, OnFail);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        var message = $"<size=100%><b>{current.Title}</size></b>\n<size=65%>{current.GetDescription()}</size>";
        Events.ZipCall(InterfaceEvent.OnTooltipUsed, message, position, 1);
    }
    public void OnPointerExit(PointerEventData eventData) => Events.EmptyCall(InterfaceEvent.OnTooltipUsed);

    void Refresh(Challenge challenge)
    {
        complete.SetActive(false);
        fail.SetActive(false);

        current = challenge;
        icon.sprite = challenge.Icon;
    }

    void OnCompletion() 
    {
        challengeDone.Play(EventArgs.Empty);
        complete.SetActive(true); 
    }
    void OnFail()
    {
        challengeFailed.Play(EventArgs.Empty);
        fail.SetActive(true);
    }
}