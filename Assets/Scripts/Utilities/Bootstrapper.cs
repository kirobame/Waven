using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Flux.Data;
using Flux;
using Flux.Event;
using UnityEngine.InputSystem;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Player[] players;
    [SerializeField] public float turnDuration;

    private Session session;
    
    //------------------------------------------------------------------------------------------------------------------/

    void Awake()
    {
        if (!Repository.Exists(References.Data)) Repository.Register(References.Data, new PlayerData[2] { PlayerData.Empty, PlayerData.Empty });
        Repository.Register(References.Players, new Player[2]);
        
        Events.Open(GameEvent.OnPlayerDeath);
        Events.Open(GameEvent.OnTurnStart);
        Events.Open(GameEvent.OnTurnTimer);
        Events.Open(GameEvent.OnTileChange);
        Events.Open(GameEvent.OnSpellUsed);
            
        Events.Open(InterfaceEvent.OnSpellSelected);
    }
    
    void Start()
    {
        var match = new Match();
        foreach (var player in players)
        {
            var turn = new TimedTurn(turnDuration);
            turn.AssignTarget(player);
            match.Insert(turn);
        }
        
        session = new Session();
        session.Add(match);

        Repository.Get<Map>(References.Map).SpawnBordermap();
    }
}