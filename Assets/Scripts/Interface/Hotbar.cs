using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private RectTransform rectTransform => (RectTransform)transform;
    [SerializeField] List<GameObject> spellUIs = new List<GameObject>();

    public void DisplaySpells(List<SpellBase> spells)
    {
        ClearSpells();
        rectTransform.sizeDelta = new Vector2(200 * spells.Count, 200);

        for (var i = 0; i < spells.Count; i++)
        {
            spellUIs[i].SetActive(true);
            spellUIs[i].GetComponent<Image>().sprite = spells[i].Thumbnail;
            spellUIs[i].GetComponent<SpellButton>().actualSpell = spells[i];
        }
    }

    public void ClearSpells()
    {
        for (var i = 0; i < spellUIs.Count; i++)
        {
            spellUIs[i].SetActive(false);
        }
    }
}
