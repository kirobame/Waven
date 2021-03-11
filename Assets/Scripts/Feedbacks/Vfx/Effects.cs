using Flux.Data;
using UnityEngine;

public class Effects : MonoBehaviour
{
    [SerializeField] private VfxGiver[] givers;
     
    void Awake() { foreach (var giver in givers) Repository.Register(giver.Key, giver); }
}