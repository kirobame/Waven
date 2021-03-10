using Flux;
using Flux.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfos : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private string playerTag;

    [SerializeField] private RectTransform playerSprite;
    //[SerializeField] private TMP_Text playerName;

    //[SerializeField] private VerticalLayoutGroup debuffZone;

    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text lifeText;
    private PlayerDamageable playerLife;

    [SerializeField] private TMP_Text deckSize;

    [SerializeField] private List<GameObject> buffs = new List<GameObject>();

    #region Active values
    private Vector2 a_playerProfileRect = new Vector2(210, 310);
   
    private Vector2 a_playerSpriteRect = new Vector2(270, 575);
    private Vector2 a_playerSpritePos = new Vector2(-21, -140);
    #endregion

    #region Inactive values
    private Vector2 i_playerProfileRect = new Vector2(160, 260);

    private Vector2 i_playerSpriteRect = new Vector2(200, 425);
    private Vector2 i_playerSpritePos = new Vector2(-11, -100);
    #endregion

    //------------------------------------------------------------------------------------------------------------------/

    private void Start()
    {
        Events.RelayByValue<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        Events.Register(GameEvent.OnPlayerDeath, OnPlayerDeath);
        playerLife = player.GetComponent<PlayerDamageable>();
        Refresh();
    }

    void OnTurnStart(Turn turn)
    {
        if (turn.Target.Name.Contains(playerTag))
        {
            if(playerTag == "1")
            {
                a_playerSpritePos = new Vector2(-21, -140);

                SetActiveSizes();
            }
            else if (playerTag == "2")
            {
                a_playerSpritePos = new Vector2(21, -140);

                SetActiveSizes();
            }
        }
        else
        {
            if (playerTag == "1")
            {
                i_playerSpritePos = new Vector2(-11, -100);

                SetInactiveSizes();
            }
            else if (playerTag == "2")
            {
                i_playerSpritePos = new Vector2(11, -100);

                SetInactiveSizes();
            }
        }
    }

    //------------------------------------------------------------------------------------------------------------------/

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

        var fullDeck = player.GetComponent<SpellDeck>().Spells.Count();
        deckSize.text = fullDeck.ToString();
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
                buffs[index].transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = value.ToString();
            }
        }
        return;
    }

    //------------------------------------------------------------------------------------------------------------------/

    private void OnPlayerDeath(EventArgs obj)
    {
        lifeText.text = $"{0}/{playerLife.Lives[0].maxValue}";
        slider.value = 0;
    }

    //------------------------------------------------------------------------------------------------------------------/

    private void SetActiveSizes()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = a_playerProfileRect;

        playerSprite.sizeDelta = a_playerSpriteRect;
        playerSprite.localPosition = a_playerSpritePos;
    }

    private void SetInactiveSizes()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = i_playerProfileRect;

        playerSprite.sizeDelta = i_playerSpriteRect;
        playerSprite.localPosition = i_playerSpritePos;
    }

}
