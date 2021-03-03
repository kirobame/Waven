using System.Linq;
using UnityEngine;
using Flux.Data;
using Flux;
using UnityEngine.InputSystem;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Player[] players;
    
    private InputActionAsset inputs;
    private Session session;
    
    //------------------------------------------------------------------------------------------------------------------/

    void Start()
    {
        var match = new Match();
        foreach (var player in players)
        {
            var turn = new TimedTurn(6.5f);
            turn.AssignTarget(player);
            match.Insert(turn);
        }
        
        session = new Session();
        session.Add(match);
        
        Routines.Start(Routines.DoAfter(() =>
        {
            var map = Repository.Get<Map>(References.Map);
            players[0].GetComponent<Navigator>().Place(map.Tiles.First().Key);
            players[1].GetComponent<Navigator>().Place(map.Tiles.Last().Key);

            inputs = Repository.Get<InputActionAsset>(References.Inputs);
            inputs.Enable();

        }, new YieldFrame()));
    }

    void OnDestroy() => inputs.Disable();
}