using System;
using UnityEngine;

[Serializable]
public class IntCastArgs : CastArgs
{
    public int Value => value;
    [SerializeField] private int value;
}