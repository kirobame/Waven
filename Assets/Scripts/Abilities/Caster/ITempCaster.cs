using System.Collections.Generic;
using Flux;

public interface ITempCaster
{
    IReadOnlyDictionary<Id, CastArgs> Args { get; }

    void Add(CastArgs args);
    bool Remove(CastArgs args);
}