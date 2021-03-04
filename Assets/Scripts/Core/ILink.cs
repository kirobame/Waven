using System;

public interface ILink
{
    event Action<ILink> onDestroyed;
    
    ITurnbound Owner { get; set; }
    
    void Activate();
    void Deactivate();
}