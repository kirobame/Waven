using System;
using Flux.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardTimer : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text texMesh;
    
    [Space, SerializeField] private Vector2Int dangerZone;
    [SerializeField] private Color textStart;
    [SerializeField] private Color fillStart;
    [SerializeField] private Color end;

    private float counter;
    
    void Awake() => Events.Register(GameEvent.OnRewardTimer, OnTimer);
    void OnDestroy() =>  Events.Unregister(GameEvent.OnRewardTimer, OnTimer);
    
    void OnTimer(EventArgs args)
    {
        if (args is WrapperArgs<bool> boolArgs)
        {
            if (boolArgs.ArgOne) image.fillAmount = 1.0f;
            else image.fillAmount = 0.0f;
        }
        else if (args is WrapperArgs<float> floatArgs)
        {
            image.fillAmount = 1.0f - floatArgs.ArgOne;
            
            counter = 10.0f - floatArgs.ArgOne * 10.0f;
            var remainingTime = Mathf.RoundToInt(counter);
            texMesh.text = remainingTime < 10 ? $"0{remainingTime}" : remainingTime.ToString();

            var ratio = Mathf.InverseLerp(dangerZone.x, dangerZone.y, remainingTime);
            texMesh.color = Color.Lerp(end, textStart, ratio);
            image.color = Color.Lerp(end, fillStart, ratio);
        }
    }
}