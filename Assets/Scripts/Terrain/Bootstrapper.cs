using System.Linq;
using UnityEngine;
using Flux.Data;
using Flux;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Navigator nav;

    private void Start()
    {
        Routines.Start(Routines.DoAfter(() =>
        {
            var map = Repository.Get<Map>(References.Map);
            nav.Place(map.Tiles.First().Key);
        }, new YieldFrame()));
        
    }
}