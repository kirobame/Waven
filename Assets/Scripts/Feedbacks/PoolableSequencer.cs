using System;
using Flux;
using Flux.Data;
using Flux.Event;
using Flux.Feedbacks;

public class PoolableSequencer : Poolable<Sequencer>
{
    public ISendback Play()
    {
        var sendbackArgs = new SendbackArgs();
        sendbackArgs.onDone += OnDone;
        
        Value.Play(sendbackArgs);
        return sendbackArgs;
    }

    void OnDone(EventArgs args) => gameObject.SetActive(false);
}