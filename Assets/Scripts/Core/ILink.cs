using System;

public interface ILink
{
    event Action<ILink> onDestroyed;
    
    ITileable Owner { get; set; }
    
    void Activate();
    void Deactivate();
}