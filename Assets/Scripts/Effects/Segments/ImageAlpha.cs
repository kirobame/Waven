using System;
using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

[Serializable, Path("UI")]
public class ImageAlpha : Segment
{
    [SerializeField] private Image image;
    [SerializeField] private float start;
    [SerializeField] private float end;

    protected override void Execute(float ratio)
    {
        var color = image.color;
        color.a = Mathf.Lerp(start, end, ratio);
        image.color = color;
    }
}
