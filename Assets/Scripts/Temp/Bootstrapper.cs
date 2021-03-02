/*using UnityEngine;
using UnityEngine.InputSystem;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputs;
    
    void Start()
    {
        var match = new Match();
        
        var turn = new TimedTurn(3.5f);
        turn.AssignTarget(new AltPlayer("Jhonny", 5));
        match.Insert(turn);
        
        turn = new TimedTurn(4.5f);
        turn.AssignTarget(new AltPlayer("Schmidt", 2));
        match.Insert(turn);
        
        turn = new TimedTurn(2.5f);
        turn.AssignTarget(new AltPlayer("Hegel", 6));
        match.Insert(turn);
        
        var session = new Session();
        session.Add(match);
    }

    void OnEnable() => inputs.Enable();
    void OnDisable() => inputs.Disable();
}
*/