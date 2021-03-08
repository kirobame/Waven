using Flux;
using Flux.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class Popup : PoolableSequencer
{
    [SerializeField] private TMP_Text textMesh;
    
    public ISendback Play(string text, StatType type)
    {
        var stats = Repository.Get<Stats>(References.Stats);
        var info = stats.Values[type];

        textMesh.text = text;
        textMesh.color = info.Color;

        return Play();
    }
}