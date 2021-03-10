using System.Collections.Generic;
using Flux;

public interface IAttributeHolder
{
    IReadOnlyDictionary<Id, List<CastArgs>> Args { get; }
    PopupAnchor PopupAnchor { get; }

    void Add(CastArgs args);
    bool Remove(CastArgs args);
    bool RemoveAll(Id id);
}