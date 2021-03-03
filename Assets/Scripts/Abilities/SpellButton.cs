using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Spell actualSpell;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Spell " + actualSpell.name + " Selected");
        Events.ZipCall<Spell>(SpellEvents.OnSpellSelected, actualSpell);
    }
}
