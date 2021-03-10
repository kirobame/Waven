using System;
using Flux.Data;
using Flux.Event;
using UnityEngine;

public class ChallengeHandler : MonoBehaviour, ILink
{
    public event Action<ILink> onDestroyed;
    
    public ITurnbound Owner { get; set; }
    public bool IsActive { get; private set; }

    [SerializeField] private Player target;

    private Challenge current;

    void OnDestroy() => onDestroyed?.Invoke(this);
    
    public void Activate()
    {
        target.WasSuccessful = false;
        IsActive = true;
        
        var challenges = Repository.Get<Challenges>(References.Challenges);
        current = challenges.PickFor(Player.Active);

        current.onCompleted += OnCompleted;
        current.onFailed += OnFailed;

        Events.ZipCall(InterfaceEvent.OnChallengeUpdate, current);
    }
    public void Deactivate()
    {
        if (!IsActive) return;
        
        current.TurnOff();
        End();
    }

    private void End()
    {
        if (!IsActive) return;

        IsActive = false;
        
        current.onCompleted -= OnCompleted;
        current.onFailed -= OnFailed;
        current = null;
    }

    void OnCompleted()
    {
        target.WasSuccessful = true;
        Events.EmptyCall(InterfaceEvent.OnChallengeCompleted);
        
        current.TurnOff();
        End();
    }
    void OnFailed()
    {
        target.WasSuccessful = false;
        Events.EmptyCall(InterfaceEvent.OnChallengeFailed);
        
        current.TurnOff();
        End();
    }
}