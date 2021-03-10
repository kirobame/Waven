using System;
using System.Collections.Generic;
using Flux;
using UnityEngine;

[Serializable]
public class SlidePush : Push
{
    protected override Vector2Int GetOrientationFor(ITileable target, int force) => Buffer.slideDirections.Dequeue();
}