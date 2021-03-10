using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private RectTransform rectTransform => (RectTransform)transform;
    [SerializeField] SpellHolder[] relays;

    [SerializeField] private TMP_Text PAText;
    [SerializeField] private TMP_Text PMText;

    public void DisplaySpells(List<SpellBase> spells)
    {
        ClearSpells();
        
        for (var i = 0; i < spells.Count; i++)
        {
            relays[i].gameObject.SetActive(true);
            relays[i].Set(spells[i]);
        }
    }

    public void DisplayPA(int remainingSpells) { PAText.text = remainingSpells.ToString(); }

    public void DisplayPM(int remainingMovements) { PMText.text = remainingMovements.ToString(); }

    public void ClearSpells() { foreach(var relay in relays) relay.gameObject.SetActive(false); }
}
