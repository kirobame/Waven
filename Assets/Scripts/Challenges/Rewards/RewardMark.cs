using System;
using UnityEngine;
using UnityEngine.UI;

public class RewardMark : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private Image image;
    
    [Space, SerializeField] private Sprite tierOne;
    [SerializeField] private Sprite tierTwo;
    [SerializeField] private Sprite tierThree;

    public void Setup(int index, int tier)
    {
        if (tier == 1) frame.sprite = tierOne;
        else if (tier == 2) frame.sprite = tierTwo;
        else frame.sprite = tierThree;
    }

    public void Show() => image.enabled = true;
    public void Hide() => image.enabled = false;
}