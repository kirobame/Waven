using System;
using System.Collections;
using System.Collections.Generic;
using Flux.Data;
using Flux.Feedbacks;
using UnityEngine;

public class SequencerPool : Pool<Sequencer, PoolableSequencer>
{
    #region Nested Types

    [Serializable]
    private class SequencerProvider : Provider<Sequencer, PoolableSequencer> { }

    #endregion

    protected override IList<Provider<Sequencer, PoolableSequencer>> Providers => providers;
    [SerializeField] private SequencerProvider[] providers;
}