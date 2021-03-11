using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using Flux.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private RectTransform rectTransform => (RectTransform)transform;
    
    [SerializeField] SpellHolder[] relays;

    [Space, SerializeField] private TMP_Text PAText;
    [SerializeField] private TMP_Text PMText;
    
    private Moveable moveable;

    void Awake() => Events.RelayByVoid(InterfaceEvent.OnInfoRefresh, Refresh);
    void OnDestroy() => Events.BreakVoidRelay(InterfaceEvent.OnInfoRefresh, Refresh);

    public void ClearSpells() { foreach(var relay in relays) relay.gameObject.SetActive(false); }
    public void DisplaySpells(List<SpellBase> spells)
    {
        ClearSpells();
        
        for (var i = 0; i < spells.Count; i++)
        {
            relays[i].gameObject.SetActive(true);
            relays[i].Set(spells[i]);
        }
    }
    
    void Refresh() => DisplayPM(((Moveable)Player.Active.Navigator).PM);

    public void DisplayPA(int remainingSpells) { PAText.text = remainingSpells.ToString(); }
    public void DisplayPM(int remainingMovements) { PMText.text = remainingMovements.ToString(); }
}
