using Flux;
using Flux.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SelfAnim : Effect
{
    [SerializeField] private Id id;
    
    private SendbackArgs sendback;
    
    protected override void ApplyTo(Tile source, IEnumerable<Tile> tiles, IReadOnlyDictionary<Id, List<CastArgs>> args)
    {
        sendback = new SendbackArgs();
        sendback.onDone += OnFeedbackDone;
        
        var relay = Buffer.caster.GetComponent<SequenceRelay>();
        if (!relay.TryPlay(id, sendback)) OnFeedbackDone(EventArgs.Empty);
    }

    void OnFeedbackDone(EventArgs args)
    {
        sendback.onDone -= OnFeedbackDone;
        End();
    }
}