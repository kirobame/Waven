using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour
{
    #region Nested Types

    [Serializable]
    public class LocalTransform
    {
        public Vector3 position;
        public float scale;
    }

    #endregion
    
    private static Dictionary<int, LocalTransform> transforms = new Dictionary<int, LocalTransform>();
    
    [SerializeField] private int index;
    [SerializeField] private LocalTransform local;

    void Awake()
    {
        if (transforms.ContainsKey(index)) return;
        transforms.Add(index, local);
    }

    void Update()
    {
        if (!transforms.TryGetValue(index, out var transform)) return;

        this.transform.localPosition = transform.position;
        this.transform.localScale = transform.scale * Vector3.one;
    }
}
