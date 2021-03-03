using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Waven/Spell")]
public class Spell : ScriptableObject, ICastable
{
    public string Title => title;
    [SerializeField] private string title;

    public string Description => description;
    [SerializeField, TextArea] private string description;

    public Sprite Thumbnail => thumbnail;
    [SerializeField] private Sprite thumbnail;


    [SerializeReference] List<Effect> effects = new List<Effect>();
    [SerializeField] Effect effect;
    public List<Effect> Effects() { return effects; }

    [SerializeField] uint cost = 0;
    public uint Cost() { return cost; }

    [SerializeField] bool canBeCastAnywhere = false;


}
