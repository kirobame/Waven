using Flux;
using Flux.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flux.Data;
using Flux.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfos : MonoBehaviour
{
    [SerializeField] private Player player;
    
    [Space, SerializeField] private Sequencer on;
    [SerializeField] private Sequencer off;
    [SerializeField] private Sequencer dead;

    [Space, SerializeField] private Image image;
    [SerializeField] private TMP_Text titleMesh;
    
    [Space, SerializeField] private Slider health;
    [SerializeField] private TMP_Text healthMesh;

    [Space, SerializeField] private TMP_Text deckSizeMesh;
    [SerializeField] private List<PlayerBuff> buffs;

    private bool state;
    
    private IDamageable damageable;
    private int cachedMaxHealth;

    private IAttributeHolder attributes;
    private Dictionary<StatType, PlayerBuff> keyedBuffs;

    private SpellDeck deck;

    void Awake()
    {
        state = false;
        
        Events.RelayByValue<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        Events.RelayByValue<Player>(GameEvent.OnPlayerDeath, OnPlayerDeath);
        Events.RelayByVoid(InterfaceEvent.OnInfoRefresh, Refresh);
        
        keyedBuffs = new Dictionary<StatType, PlayerBuff>();
        foreach (var buff in buffs)
        {
            if (keyedBuffs.ContainsKey(buff.Type)) continue;
            keyedBuffs.Add(buff.Type, buff);
        }
        
        damageable = player.GetComponent<IDamageable>();
        cachedMaxHealth = damageable.Get("Health").maxValue;

        attributes = player.GetComponent<IAttributeHolder>();

        deck = player.GetComponent<SpellDeck>();
    }

    void OnDestroy()
    {
        Events.BreakValueRelay<Turn>(GameEvent.OnTurnStart, OnTurnStart);
        Events.BreakValueRelay<Player>(GameEvent.OnPlayerDeath, OnPlayerDeath);
        Events.BreakVoidRelay(InterfaceEvent.OnInfoRefresh, Refresh);
    }

    void OnTurnStart(Turn turn)
    {
        if (!(turn.Target is Player player)) return;
        
        if (this.player != player)
        {
            if (state)
            {
                off.Play(EventArgs.Empty, on);
                state = false;
            }
            
            return;
        }
        else
        {
            if (!state)
            {
                on.Play(EventArgs.Empty, off);
                state = true;
            }
        }
    }

    void OnPlayerDeath(Player player)
    {
        if (this.player != player) return;

        health.value = 0;
        healthMesh.text = $"0/{cachedMaxHealth}";
        
        dead.Play(EventArgs.Empty, on, off);
    }

    void Refresh()
    {
        var life = damageable.Get("Health");
        health.value = (float)life.actualValue / cachedMaxHealth;
        healthMesh.text = $"{life.actualValue}/{cachedMaxHealth}";
        
        HandleBuff(new Id('M','V','T'), StatType.Movement);
        HandleBuff(new Id('D','M','G'), StatType.Damage);
        HandleBuff(new Id('P','S','H'), StatType.Force);

        var spellsCount = deck.Spells.Count();
        deckSizeMesh.text = spellsCount < 10 ? $"0{spellsCount}" : spellsCount.ToString();
    }

    private void HandleBuff(Id id, StatType type)
    {
        if (!attributes.Args.TryGetValue(id, out var args))
        {
            HideBuff(type);
            return;
        }

        var value = 0;
        foreach (var arg in args)
        {
            if (!(arg is IWrapper<int> wrapper) || arg is FoundingWrapperCastArgs<int>) continue;
            value += wrapper.Value;
        }

        if (value != 0)
        {
            var buff = keyedBuffs[type];
            if (!buff.IsActive) buff.Show(value);
        }
        else HideBuff(type);
    }

    private void HideBuff(StatType type)
    {
        var buff = keyedBuffs[type];
        if (buff.IsActive) buff.Hide();
    }

    /*[SerializeField] private Player player;
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
        //Refresh();
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
        if (player == null) return;
        
        if (playerLife.IsAlive)
        {
            var currentLife = playerLife.Get("Health");
            var ratio = currentLife.Ratio;
            slider.value = ratio;

            lifeText.text = $"{playerLife.Lives[0].actualValue}/{playerLife.Lives[0].maxValue}";
        }
        
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
        /*lifeText.text = $"{0}/{playerLife.Lives[0].maxValue}";
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
    }*/
}
