using System;

public interface IBusy
{
    event Action onFree;
    
    bool IsBusy { get; }
}
public interface ITurnbound : IBusy
{
    event Action<Motive> onIntendedTurnStop;
    
    string Name { get; }

    Match Match { set; }
    short Initiative { get; }
    
    void Activate();
    void Interrupt(Motive motive);
}