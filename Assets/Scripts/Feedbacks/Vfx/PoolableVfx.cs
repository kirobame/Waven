using System;
using System.Collections;
using Flux.Data;
using UnityEngine;

public class PoolableVfx : Poolable<Animator>
{
     public event Action<PoolableVfx> onDone;
     
     [SerializeField] private string outTag;
     [SerializeField] private string inTrigger;

     public void Play() => Value.SetTrigger(inTrigger);

     void Update()
     {
          var info = Value.GetCurrentAnimatorStateInfo(0);
          if (info.IsTag(outTag))
          {
               onDone?.Invoke(this);
               gameObject.SetActive(false);
          }
     }
}
