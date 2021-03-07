using Flux.Data;
using Flux.Event;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoverSignal : MonoBehaviour
{
    public static int activeId;
    
    [SerializeField] private GameObject root;

    [Space, SerializeField] private InfoAnchor anchor;
    [SerializeField] private SpriteRenderer shape;

    private bool previousOverlap;

    void Awake() => activeId = 0;
    void OnDestroy()
    {
        if (!previousOverlap) return;
        
        Events.ZipCall(InterfaceEvent.OnHoverEnd, root);
        activeId = 0;
    }

    void Update()
    {
        var id = GetInstanceID();
        if (activeId != 0 && activeId != id) return;
        
        var camera = Repository.Get<Camera>(References.Camera);
        
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = -camera.transform.position.z;

        var overlap = shape.bounds.Contains(camera.ScreenToWorldPoint(mousePosition));

        if (overlap == true && previousOverlap == false)
        {
            Events.ZipCall(InterfaceEvent.OnHoverStart, anchor, root);
            activeId = id;
        }
        else if (overlap == false && previousOverlap == true)
        {
            Events.ZipCall(InterfaceEvent.OnHoverEnd, root);
            activeId = 0;
        }

        previousOverlap = overlap;
    }
}