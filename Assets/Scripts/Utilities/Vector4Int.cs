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
    
    public int Left => left;
    [SerializeField] private int left;
    
    public int Right => right;
    [SerializeField] private int right;
    
    public int Up => up;
    [SerializeField] private int up;
    
    public int Down => down;
    [SerializeField] private int down;
}