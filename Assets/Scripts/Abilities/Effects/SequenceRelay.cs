using System;
using System.Collections.Generic;
using Flux;
using Flux.Feedbacks;
using UnityEngine;

public class SequenceRelay : MonoBehaviour
{
    #region Nested Types

    [Serializable]
    private class IdSequencerPair : Flux.KeyValuePair<Id,Sequencer> { }

    #endregion

    [SerializeField] private IdSequencerPair[] pairs;
    private Dictionary<Id, Sequencer> registry;

    void Awake()
    {
        registry = new Dictionary<Id, Sequencer>();

        foreach (var pair in pairs)
        {
            if (registry.ContainsKey(pair.Key)) continue;
            registry.Add(pair.Key, pair.Value);
        }
    }

    public bool TryPlay(Id id, EventArgs args)
    {
        if (registry.TryGetValue(id, out var sequencer))
        {
            sequencer.Play(args);
            return true;
        }

        return false;
    }
}