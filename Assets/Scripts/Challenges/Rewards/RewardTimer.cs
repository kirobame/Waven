using System;
using Flux.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardTimer : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text texMesh;

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
            texMesh.text = $"{(int)Mathf.RoundToInt(counter)}";
        }
    }
}