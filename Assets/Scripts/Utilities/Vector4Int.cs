using System;
using UnityEngine;

[Serializable]
public struct Vector4Int
{
    public Vector4Int(int left, int right, int up, int down)
    {
        this.left = left;
        this.right = right;
        this.up = up;
        this.down = down;
    }

    public int left;
    public int right;
    public int up;
    public int down;
}