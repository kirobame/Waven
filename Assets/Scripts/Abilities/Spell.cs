using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using UnityEngine;


public class Spell : MonoBehaviour
{
    public delegate void EffectDelegate(List<Vector3Int> tiles);
    EffectDelegate[] effects = { Effect1, Effect2 };
    public EffectDelegate[] Effects() { return effects; }


    [SerializeField] uint cost = 0;
    public uint Cost() { return cost; }

    [SerializeField] bool canBeCastAnywhere = false;

    static void Effect1(List<Vector3Int> tiles)
    {
        //Fonction pour GetEntitiesOnTiles

    }
    static void Effect2(List<Vector3Int> tiles)
    {

    }

}
