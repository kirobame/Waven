using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux.Data;
using Flux.Event;
using Flux.Feedbacks;
using UnityEngine;

public class RewardHandler : MonoBehaviour
{
    public event Action onDone;
    public event Action onHideStart;

    public bool IsHiding { get; private set; }

    public SpellDeck Current => current;
    public int Tier
    {
        get
        {
            var tier = 1;
            if (start > limits.x) tier++;
            if (start > limits.y) tier++;

            return tier;
        }
    }
    
    [SerializeField] private CanvasGroup group;
    
    [Space, SerializeField] private RewardMark[] marks; 
    [SerializeField] private Vector2Int limits;
    [SerializeField] private RewardChoice[] choices;

    [Space, SerializeField] private Sequencer entry;
    [SerializeField] private Sequencer exit;

    private SendbackArgs showArgs = new SendbackArgs();
    private SendbackArgs hideArgs = new SendbackArgs();

    private SpellDeck current;
    private int start;

    void Awake()
    {
        Repository.Register(References.Reward, this);
        
        showArgs.onDone += OnShown;
        hideArgs.onDone += OnHidden;
        
        for (var i = 0; i < marks.Length; i++)
        {
            if (i < limits.x) marks[i].Setup(i, 1);
            else if (i < limits.y) marks[i].Setup(i, 2);
            else marks[i].Setup(i, 3);
        }
    }
    void OnDestroy() => Repository.Unregister(References.Reward);

    public void ShowFor(Player player)
    {
        current = player.GetComponent<SpellDeck>();
        var spells = current.Spells.ToArray();

        start = spells.Length - 3;
        for (var i = 0; i < spells.Length - 3; i++) marks[i].Show();
        for (var i = start; i < marks.Length; i++) marks[i].Hide();

        var tier = Tier;
        foreach (var choice in choices) choice.Initialize(tier);
        
        entry.Play(showArgs);
    }
    public void Hide()
    {
        Debug.Log("HIDING REWARDS");
        if (IsHiding) return;
        
        onHideStart?.Invoke();
        IsHiding = true;
        
        group.blocksRaycasts = false;
        exit.Play(hideArgs);
    }

    public void Pick(SpellBase spell)
    {
        marks[start].Show();
        current.Add(spell);
        
        Hide();
    }

    void OnShown(EventArgs args) => group.blocksRaycasts = true;
    void OnHidden(EventArgs args)
    {
        onDone?.Invoke();
        IsHiding = false;
    }
}