﻿using UnityEngine;
using UnityEngine.UI;

public class SpellHolder : MonoBehaviour
{
    public SpellBase Value => value;
    [SerializeField] private SpellBase value;

    [SerializeField] private Image image;

    public void Set(SpellBase value)
    {
        this.value = value;
        image.sprite = value.Thumbnail;
    }
}