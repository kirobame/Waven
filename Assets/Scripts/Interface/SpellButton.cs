using System.Collections;
using System.Collections.Generic;
using Flux;
using Flux.Event;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    public SpellBase actualSpell;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Spell " + actualSpell.name + " Selected");
        Events.ZipCall<SpellBase>(InterfaceEvent.OnSpellSelected, actualSpell);
    }
}
