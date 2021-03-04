using System.Collections.Generic;
using Flux;

public interface IAttributeHolder
{
    IReadOnlyDictionary<Id, CastArgs> Args { get; }

    void Add(CastArgs args);
    bool Remove(CastArgs args);
}