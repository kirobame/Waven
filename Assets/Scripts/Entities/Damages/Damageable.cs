using System;
using System.Collections;
using Flux.Event;
using System.Collections.Generic;
using System.Linq;
using Flux;
using Flux.Data;
using Flux.Audio;
using Flux.Feedbacks;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public event Action<IDamageable> onFeedbackDone;

    public bool IsAlive => lives.Any();
    public bool IsInvulnerable { get; set; }
    public bool IsFeedbackDone { get; private set; }

    public List<Life> Lives => lives;
    
    public TeamTag Team
    {
        get => tag.Team;
        set => tag.Team = value;
    }
    private new Tag tag;

    [SerializeField] protected PopupAnchor popupAnchor;
    [SerializeField] private List<Life> lives= new List<Life>()
    {
        new Life("Health", new Color(0.2812389f, 0.754717f, 0.303109f, 1.0f), 10, 10, 0, new List<DamageType>() { DamageType.Base })
    };

    private Coroutine popupRoutine;
    private List<(int damage, DamageType type)> history = new List<(int damage, DamageType type)>();

    [SerializeField] private AudioClipPackage hurtSound;

    //------------------------------------------------------------------------------------------------------------------/

    protected virtual void Awake()
    {
        tag = GetComponent<Tag>();
        lives.Sort();

        IsFeedbackDone = true;
        onFeedbackDone += self => IsFeedbackDone = true;
    }

    //------------------------------------------------------------------------------------------------------------------/
    
    public Life Get(string name) => lives.FirstOrDefault(life => life.name.ToLower().Equals(name.ToLower()));
    public void AddLife(Life value)
    {
        lives.Add(value);
        lives.Sort();
    }

    public int Inflict(int damage, DamageType type)
    {
        var index = 0;

        if (IsInvulnerable)
        {
            EndFeedback();
            return 0;
        }
        if (!lives.Any())
        {
            EndFeedback();
            return 2;
        }
        
        OnDamageTaken(damage, type);

        while (damage > 0)
        {
            if (!lives[index].HandledTypes.Contains(type)) return 1;

            lives[index].actualValue -= damage;
            if (lives[index].actualValue <= 0)
            {
                damage = -lives[index].actualValue;
                lives.RemoveAt(index);

                if (!lives.Any())
                {
                    Events.EmptyCall(ChallengeEvent.OnKill);
                    OnDeath();
                    
                    return 2;
                }
            }
            else
            {
                Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
                Events.ZipCall(ChallengeEvent.OnDamage, this);
                if (IsFeedbackDone)
                {
                    OnLogicDone();
                    IsFeedbackDone = false;
                }

                return 3;
            }
        }

        Events.EmptyCall(InterfaceEvent.OnInfoRefresh);
        Events.ZipCall(ChallengeEvent.OnDamage, this);
        if (IsFeedbackDone)
        {
            OnLogicDone();
            IsFeedbackDone = false;
        }
        
        return 3;
    }
    
    //------------------------------------------------------------------------------------------------------------------/

    protected virtual void OnDamageTaken(int damage, DamageType type)
    {
        if (history.Count == 0 && popupRoutine == null) StartCoroutine(PopupRoutine());
        AudioHandler.Play(hurtSound);
        history.Add((damage, type));
    }

    protected virtual void OnLogicDone() => EndFeedback();
    protected void EndFeedback() => onFeedbackDone?.Invoke(this);
    
    protected virtual void OnDeath()
    {
        if (popupRoutine != null)
        {
            StopCoroutine(popupRoutine);
            popupRoutine = null;
        }
        
        if (history.Count > 0) ShowPopup();
        popupAnchor.Clear();
        
        EndFeedback();
        Destroy(gameObject);
    }
    
    private IEnumerator PopupRoutine()
    {
        for (var i = 0; i < 3; i++) yield return new WaitForEndOfFrame();

        ShowPopup();
        popupRoutine = null;
    }
    private void ShowPopup()
    {
        var pool = Repository.Get<SequencerPool>(Pools.Popup);
        var popup = pool.RequestSinglePoolable() as Popup;

        popup.transform.SetParent(popupAnchor.transform);
        popup.transform.localPosition = Vector3.zero;

        var damage = 0;
        foreach (var item in history) damage += item.damage;
        popup.Play($"-{damage}");
        
        history.Clear();
    }
}