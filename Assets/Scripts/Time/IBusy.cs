using System;

public interface IBusy
{
    event Action onFree;
    
    bool IsBusy { get; }
}