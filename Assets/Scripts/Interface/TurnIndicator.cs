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
    [SerializeField] private Image timer;
    [SerializeField] private TMP_Text timerValue;

    private Bootstrapper boot;

    //------------------------------------------------------------------------------------------------------------------/
    
    void Awake()
    {
        Events.RelayByValue<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        Events.Register(GameEvent.OnTurnTimer, OnTurnTimer);
    }
    void Start() => boot = Repository.Get<Bootstrapper>(References.Bootstrapper);

    void OnDestroy()
    {
        Events.BreakValueRelay<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        Events.Unregister(GameEvent.OnTurnTimer, OnTurnTimer);
    }

    //------------------------------------------------------------------------------------------------------------------/

    void OnTurnStart(Turn turn)
    {
        timerValue.color = new Color32(77, 77, 77, 255);

        if (turn.Target is Component component)
        {
            hasTarget = true;
            target = component.transform;
        }
        else hasTarget = false;*/
    }
    void OnTurnTimer(EventArgs args)
    {
        if (args is WrapperArgs<bool> boolArgs)
        {
            if (boolArgs.ArgOne) timer.fillAmount = 1.0f;
            else timer.fillAmount = 0.0f;
        }
        else if (args is WrapperArgs<float> floatArgs)
        {
            timer.fillAmount = 1.0f - floatArgs.ArgOne;

            var remainingTime = (int)(boot.turnDuration - (floatArgs.ArgOne * boot.turnDuration));
            timerValue.text = $"{remainingTime}";

            if(remainingTime <= 10) { timerValue.color = new Color32(221, 0, 43, 255); }
        }
    }
}
