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
        Buffer.hasStopped = false;
        Time.timeScale = 1;
        
        if (!Repository.Exists(References.Data)) Repository.Register(References.Data, new PlayerData[2] { PlayerData.Empty, PlayerData.Empty });
        Repository.Register(References.Players, new Player[2]);

        Events.Open(GameEvent.OnPlayerDeath);
        Events.Open(GameEvent.OnTileChange);
        Events.Open(GameEvent.OnSpellUsed);
        
        Events.Open(GameEvent.OnTurnStart);
        Events.Open(GameEvent.OnTurnTimer);
        Events.Open(GameEvent.OnTurnEnd);
        
        Events.Open(GameEvent.OnRewardStart);
        Events.Open(GameEvent.OnRewardTimer);
        Events.Open(GameEvent.OnRewardEnd);
            
        Events.Open(InterfaceEvent.OnSpellInterruption);
        Events.Open(InterfaceEvent.OnHideTooltip);
        Events.Open(InterfaceEvent.OnSpellSelected);
        
        Events.RelayByVoid(GameEvent.OnTurnStart, OnTurnStart);
    }
    void OnDestroy() => Events.BreakVoidRelay(GameEvent.OnTurnStart, OnTurnStart);
    
    void Start()
    {
        Repository.Get<Spells>(References.Spells).Bootup();

        var match = new Match();
        foreach (var player in players)
        {
            var turn = new TimedTurn(turnDuration, GameEvent.OnTurnStart, GameEvent.OnTurnTimer, GameEvent.OnTurnEnd);
            turn.AssignTarget(player);
            match.Insert(turn);
            
            var rewardRelay = new RewardRelay((short)(player.Initiative + 1));
            rewardRelay.SetTarget(player);
            
            var rewardTurn = new TimedTurn(10.0f, GameEvent.OnRewardStart, GameEvent.OnRewardTimer, GameEvent.OnRewardEnd);
            rewardTurn.AssignTarget(rewardRelay);
            match.Insert(rewardTurn);
        }
        
        StartCoroutine(Routines.DoAfter(() =>
        {
            session = new Session();
            session.Add(match);

        }, new YieldFrame()));

        Repository.Get<Map>(References.Map).SpawnBordermap();
    }

    void OnTurnStart() => Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
}