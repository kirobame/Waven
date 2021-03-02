using System;

public interface ITurnbound
{
    event Action<Motive> onIntendedTurnStop;
    
    Match Match { set; }
    short Initiative { get; }
    
    void Activate();
    void Interrupt(Motive motive);
}