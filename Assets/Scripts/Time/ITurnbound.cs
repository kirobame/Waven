using System;

public interface ITurnbound : IBusy
{
    event Action<Motive> onIntendedTurnStop;
    
    string Name { get; }

    Match Match { set; }
    short Initiative { get; }
    
    void Activate();
    void Interrupt(Motive motive);
}