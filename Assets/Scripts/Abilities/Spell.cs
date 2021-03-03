using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Waven/Spell")]
public class Spell : ScriptableObject
{
    //public delegate void EffectDelegate(List<Vector3Int> tiles);
    //EffectDelegate[] effects = { Effect1, Effect2 };
    //public EffectDelegate[] Effects() { return effects; }

    [SerializeReference] List<Effect> effects = new List<Effect>();
    [SerializeField] Effect effect;
    public List<Effect> Effects() { return effects; }

    [SerializeField] uint cost = 0;
    public uint Cost() { return cost; }

    [SerializeField] bool canBeCastAnywhere = false;


}
