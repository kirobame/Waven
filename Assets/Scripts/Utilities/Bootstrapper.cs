using System.Linq;
using UnityEngine;
using Flux.Data;
using Flux;
using Flux.Event;
using UnityEngine.InputSystem;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Player[] players;
    [SerializeField] private float turnDuration;

    private Session session;
    
    //------------------------------------------------------------------------------------------------------------------/

    void Awake()
    {
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