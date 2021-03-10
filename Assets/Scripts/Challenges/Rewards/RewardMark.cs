using System;
using UnityEngine;
using UnityEngine.UI;

public class RewardMark : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private Image image;
    
    [Space, SerializeField] private Color tierOne;
    [SerializeField] private Color tierTwo;
    [SerializeField] private Color tierThree;

    public void Setup(int index, int tier)
    {
        if (tier == 1) frame.color = tierOne;
        else if (tier == 2) frame.color = tierTwo;
        else frame.color = tierThree;
    }

    public void Show() => image.enabled = true;
    public void Hide() => image.enabled = false;
}