using System;
using UnityEngine;

[Serializable]
public class GolemOwnership
{
    public Player Owner { get; set; }
    
    public bool State => state;
    [SerializeField] private bool state;

    public void Activate(Player player)
    {
        Owner = player;
        state = true;
    }
    public void Deactivate()
    {
        Owner = null;
        state = false;
    }
}