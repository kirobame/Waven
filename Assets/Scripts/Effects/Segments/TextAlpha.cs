using System;
using Flux;
using Flux.Feedbacks;
using TMPro;
using UnityEngine;

[Serializable, Path("UI")]
public class TextAlpha : Segment
{
    [SerializeField] private TMP_Text textMesh;
    [SerializeField] private float start;
    [SerializeField] private float end;

    protected override void Execute(float ratio)
    {
        var color = textMesh.color;
        color.a = Mathf.Lerp(start, end, ratio);
        textMesh.color = color;
    }
}