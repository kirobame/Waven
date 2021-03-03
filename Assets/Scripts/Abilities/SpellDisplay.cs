using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellDisplay : MonoBehaviour
{
    RectTransform spellsPanel;

    void Start()
    {
        spellsPanel = Repository.Get<RectTransform>(References.SpellsPanel);
        DisplaySpells(Repository.Get<FakeDeck>(References.Deck).deck);
    }

    public void DisplaySpells(List<FakeSpell> spells)
    {
        spellsPanel.sizeDelta = new Vector2(200 * spells.Count, 200);

        for (int i = 0; i < spells.Count; i++)
        {
            spellsPanel.GetChild(i).gameObject.SetActive(true);
            spellsPanel.GetChild(i).GetComponent<Image>().sprite = spells[i].Thumbnail;
            spellsPanel.GetChild(i).GetComponent<UISpellData>().spell = spells[i];
        }
    }
}
