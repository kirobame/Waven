using Flux;
using Flux.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfos : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private string playerTag;

    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text lifeText;
    private PlayerDamageable playerLife;

    [SerializeField] private List<GameObject> buffs = new List<GameObject>();

    private void Start()
    {
        Events.RelayByValue<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        playerLife = player.GetComponent<PlayerDamageable>();
    }

    void OnTurnStart(Turn turn)
    {
        if (turn.Target.Name.Contains(playerTag))
        {
            gameObject.SetActive(true); //set scale big
        }
        else
        {
            gameObject.SetActive(false); //set scale small
        }
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        var currentLife = playerLife.Get("Health");
        var ratio = currentLife.Ratio;
        slider.value = ratio;

        lifeText.text = $"{playerLife.Lives[0].actualValue}/{playerLife.Lives[0].maxValue}";

        if(player.TryGet<IAttributeHolder>(out var attributes))
        {
            HandleStats(attributes, new Id('M', 'V', 'T'), 0);
            HandleStats(attributes, new Id('D', 'M', 'G'), 1);
            HandleStats(attributes, new Id('P', 'S', 'H'), 2);
        }
    }

    private void HandleStats(IAttributeHolder attributes, Id id, int index)
    {
        if (attributes.Args.TryGetValue(id, out var baseValue, out var value))
        {
            if (value == 0)
            {
                buffs[index].gameObject.SetActive(false);
            }
            else
            {
                buffs[index].gameObject.SetActive(true);
                buffs[index].transform.GetChild(0).GetComponent<TMP_Text>().text = value.ToString();
            }
        }
        return;
    }
}
