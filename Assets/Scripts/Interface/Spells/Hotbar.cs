using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private RectTransform rectTransform => (RectTransform)transform;
    [SerializeField] SpellHolder[] relays;

    public void DisplaySpells(List<SpellBase> spells)
    {
        ClearSpells();
        
        for (var i = 0; i < spells.Count; i++)
        {
            relays[i].gameObject.SetActive(true);
            relays[i].Set(spells[i]);
        }
    }

    public void ClearSpells() { foreach(var relay in relays) relay.gameObject.SetActive(false); }
}
