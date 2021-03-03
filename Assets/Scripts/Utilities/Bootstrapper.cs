using System.Linq;
using UnityEngine;
using Flux.Data;
using Flux;
using Flux.Event;
using UnityEngine.InputSystem;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Player[] players;

    private Session session;
    
    //------------------------------------------------------------------------------------------------------------------/

    void Start()
    {
        Events.Open(GameEvent.OnTurnStart);
        Events.Open(GameEvent.OnTurnTimer);
        
        Events.Open(InterfaceEvent.OnSpellSelected);

        var match = new Match();
        foreach (var player in players)
        {
            var turn = new TimedTurn(10.0f);
            turn.AssignTarget(player);
            match.Insert(turn);
        }
        
        session = new Session();
        session.Add(match);
    }
}