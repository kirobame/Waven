using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using UnityEngine;

public class SpellButton : MonoBehaviour
{
    [SerializeField] Spell actualSpell;


    public void OnSelected()
    {
        Debug.Log("Spell " + actualSpell.name + " Selected");
        Events.ZipCall<Spell>(SpellEvents.OnSpellSelected, actualSpell);
    }
}
