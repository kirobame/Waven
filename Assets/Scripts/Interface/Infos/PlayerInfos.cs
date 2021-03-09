using Flux.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfos : MonoBehaviour
{
    [SerializeField] private string playerTag;

    private void Start()
    {
        Events.RelayByValue<Turn>(GameEvent.OnTurnStart, OnTurnStart);
    }

    void OnTurnStart(Turn turn)
    {
        Debug.Log(turn.Target.Name);
        if (turn.Target.Name.Contains(playerTag))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
