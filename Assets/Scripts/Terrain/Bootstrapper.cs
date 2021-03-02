using System.Linq;
using UnityEngine;
using Flux.Data;
using Flux;
using UnityEngine.InputSystem;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Navigator nav;

    void Start()
    {
        Repository.Get<InputActionAsset>(References.Inputs).Enable();
        
        Routines.Start(Routines.DoAfter(() =>
        {
            var map = Repository.Get<Map>(References.Map);
            nav.Place(map.Tiles.First().Key);
        }, new YieldFrame()));
    }

    void OnDestroy() => Repository.Get<InputActionAsset>(References.Inputs).Disable();
}