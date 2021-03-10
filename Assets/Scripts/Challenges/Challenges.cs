using Flux.Data;
using Flux.Event;
using UnityEngine;

public class Challenges : MonoBehaviour
{
    [SerializeReference] private Challenge[] challenges = new Challenge[0];

    void Awake()
    {
        Repository.Register(References.Challenges, this);
        
        Events.Open(ChallengeEvent.OnAttack);
        Events.Open(ChallengeEvent.OnDamage);
        Events.Open(ChallengeEvent.OnKill);
        Events.Open(ChallengeEvent.OnMove);
        Events.Open(ChallengeEvent.OnSpellUse);
        Events.Open(ChallengeEvent.OnPush);
    }
    void OnDestroy() => Repository.Unregister(References.Challenges);
    
    public Challenge PickFor(Player player)
    {
        var challenge = challenges[Random.Range(0, challenges.Length)];
        challenge.TurnOn(player);

        return challenge;
    }
}