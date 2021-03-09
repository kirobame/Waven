using System;
using Flux.Data;
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
        
        Debug.Log($"For : {this} - {current}");
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
        Debug.Log($"For : {this} - Challenge completed");
        target.WasSuccessful = true;
        
        current.TurnOff();
        End();
    }
    void OnFailed()
    {
        Debug.Log($"For : {this} - Challenge failed");
        target.WasSuccessful = false;
        
        current.TurnOff();
        End();
    }
}