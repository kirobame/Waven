using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamTag { Player1, Player2, Neutral }

public interface ITag
{
    TeamTag Value { get; }
}


public class Tag : MonoBehaviour, ITag
{
    public TeamTag Value => tag;
    [SerializeField] private new TeamTag tag;
}
