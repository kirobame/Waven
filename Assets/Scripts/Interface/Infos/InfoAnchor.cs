using System;
using UnityEngine;

public class InfoAnchor : MonoBehaviour
{
    public event Action onDestroyed;
    
    public Vector3 Position => transform.position;

    void OnDestroy() => onDestroyed?.Invoke();
}