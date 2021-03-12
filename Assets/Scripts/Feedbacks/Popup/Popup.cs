using Flux;
using Flux.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Popup : PoolableSequencer
{
    [SerializeField] private TMP_Text textMesh;
    [SerializeField] private SpriteRenderer icon;
    
    [Space, SerializeField] private Color normal;
    
    public ISendback Play(string text, StatType type)
    {
        var stats = Repository.Get<Stats>(References.Stats);
        var info = stats.Values[type];

        Debug.Log(info);

        icon.enabled = true;
        icon.sprite = info.Icon;
        
        textMesh.text = text;
        textMesh.color = info.Color;

        return Play();
    }

    public ISendback Play(string text)
    {
        icon.enabled = false;
        
        textMesh.color = normal;
        textMesh.text = text;
        
        return Play();
    }
}