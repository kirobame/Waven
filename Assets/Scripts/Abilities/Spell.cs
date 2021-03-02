using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using UnityEngine;

public class Spell : ScriptableObject
{
    public delegate void EffectDelegate(List<Vector2> pos);
    EffectDelegate[] effects = { Effect1, Effect2 };
    public EffectDelegate[] Effects() { return effects; }
    //List<EffectDelegate> effects = new List<EffectDelegate>();
    //public List<EffectDelegate> Effects() { return effects; }

    uint cost = 0;
    public uint Cost() { return cost; }

    bool canBeCastAnywhere = false;


    static void Effect1(List<Vector2> pos)
    {

    }
    static void Effect2(List<Vector2> pos)
    {

    }
}
