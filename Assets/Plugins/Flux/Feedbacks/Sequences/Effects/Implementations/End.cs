using System;
using UnityEngine;

namespace Flux.Feedbacks
{
    [Serializable, Path("Utilities")]
    public class End : Effect
    {
        protected override void OnUpdate(EventArgs args)
        {
            if (args is ISendback sendback) sendback.End(EventArgs.Empty);
            IsDone = true;
        }
    }
}