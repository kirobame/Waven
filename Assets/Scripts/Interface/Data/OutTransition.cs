using Flux.Data;
using Flux.Event;
using UnityEngine;

public class OutTransition : MonoBehaviour
{
    void Awake() => Events.RelayByValue<int>(GameEvent.OnPlayerDeath, OnPlayerDeath);
    void OnDestroy() => Events.BreakValueRelay<int>(GameEvent.OnPlayerDeath, OnPlayerDeath);

    void OnPlayerDeath(int index)
    {
        var players = Repository.GetAll<Player>(References.Players);
        var data = Repository.GetAll<PlayerData>(References.Data);

        for (var i = 0; i < data.Length; i++)
        {
            if (!data[i].IsValid) continue;
            
            var spellDeck = players[i].GetComponent<SpellDeck>();
            
            data[i].SetDeck(spellDeck.Spells);
            data[i].Save();
        }
        
        Events.BreakValueRelay<int>(GameEvent.OnPlayerDeath, OnPlayerDeath);
    }
}