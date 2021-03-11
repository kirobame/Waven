using System;
using Flux.Data;
using Flux.Event;
using Flux.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuff : MonoBehaviour
{
    public bool IsActive { get; private set; }
    public StatType Type => type;
    
    [SerializeField] private StatType type;

    [Space, SerializeField] private Sequencer show;
    [SerializeField] private Sequencer hide;
    [SerializeField] private string suffix;
    
    [Space, SerializeField] private Image image;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text textMesh;

    void Awake() => IsActive = false;

    void Start()
    {
        var stats = Repository.Get<Stats>(References.Stats);
        var info = stats.Values[type];

        image.color = info.Color;
        icon.sprite = info.Icon;
    }

    public void Refresh(int value) => textMesh.text = value < 10 ? $"0{value}" : value.ToString();
    
    public void Show(int value)
    {
        Refresh(value);
        
        show.Play(new WrapperArgs<string>($"Show{suffix}"), hide);
        IsActive = true;
    }
    public void Hide()
    {
        textMesh.text = "0";
        
        hide.Play(new WrapperArgs<string>($"Hide{suffix}"), show);
        IsActive = false;
    }
}