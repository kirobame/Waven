using UnityEngine;

public class PopupAnchor : MonoBehaviour
{
    public Vector3 Position => transform.position;

    public void Clear() { while (transform.childCount > 0) transform.GetChild(0).SetParent(null); }
}