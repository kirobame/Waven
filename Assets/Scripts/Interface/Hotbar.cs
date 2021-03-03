using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private RectTransform rectTransform => (RectTransform)transform;

    public void DisplaySpells(List<Spell> spells)
    {
        rectTransform.sizeDelta = new Vector2(200 * spells.Count, 200);

        for (var i = 0; i < spells.Count; i++)
        {
            rectTransform.GetChild(i).gameObject.SetActive(true);
            rectTransform.GetChild(i).GetComponent<Image>().sprite = spells[i].Thumbnail;
            rectTransform.GetChild(i).GetComponent<SpellButton>().actualSpell = spells[i];
        }
    }
}
