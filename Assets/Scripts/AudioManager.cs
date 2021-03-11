using Flux.Event;
using Flux.Data;
using Flux.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClipPackage endTurnSound;

    void Start()
    {
        Events.Register(GameEvent.OnTurnEnd, OnTurnEnd);
        Events.Register(GameEvent.OnPlayerDeath, OnPlayerDeath);
        OnGameStart();
    }

    void OnGameStart()
    {
        //Play Music
    }
    void OnTurnEnd(EventArgs args)
    {
        AudioHandler.Play(endTurnSound);
    }
    void OnPlayerDeath(EventArgs args)
    {

    }

}
