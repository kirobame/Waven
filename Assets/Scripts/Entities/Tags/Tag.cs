using UnityEngine;

public class Tag : MonoBehaviour, ITag
{
    public TeamTag Team
    {
        get => tag;
        set => tag = value;
    }
    [SerializeField] private new TeamTag tag;
}