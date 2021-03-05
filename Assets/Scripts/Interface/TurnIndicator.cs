using System;
using System.Collections;
using System.Collections.Generic;
using Flux.Data;
using Flux.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnIndicator : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private Slider timer;
    
    [Space, SerializeField] private Image subIndicator;
    [SerializeField] private float heightOffset;

    private bool hasTarget;
    private Transform target;
    
    //------------------------------------------------------------------------------------------------------------------/
    
    void Start()
    {
        Events.RelayByValue<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        Events.Register(GameEvent.OnTurnTimer, OnTurnTimer);
    }

    void Update()
    {
        if (!hasTarget) return;

        var camera = Repository.Get<Camera>(References.Camera);
        var screenPosition = camera.WorldToScreenPoint(target.position + Vector3.up * heightOffset);

        subIndicator.rectTransform.position = screenPosition;
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    void OnTurnStart(Turn turn)
    {
        title.text = turn.Target.Name;
        
        if (turn.Target is Component component)
        {
            hasTarget = true;
            target = component.transform;
        }
        else hasTarget = false;
    }
    void OnTurnTimer(EventArgs args)
    {
        if (args is WrapperArgs<bool> boolArgs)
        {
            if (boolArgs.ArgOne) timer.value = 1.0f;
            else timer.value = 0.0f;
        }
        else if (args is WrapperArgs<float> floatArgs) timer.value = 1.0f - floatArgs.ArgOne;
    }
}
