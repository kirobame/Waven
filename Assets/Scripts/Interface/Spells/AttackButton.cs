using Flux.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image button;
    [SerializeField] private SpellBase spell;
    
    private Pathfinder Pathfinder => Player.Active.GetComponent<Pathfinder>();

    private bool inUse;
    
    void Awake()
    {
        Events.RelayByValue<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
        Events.RelayByVoid(GameEvent.OnTurnStart, OnTurnStart);
        Events.RelayByVoid(ChallengeEvent.OnAttack, OnAttack);
    }
    void OnDestroy()
    {
        Events.BreakValueRelay<SpellBase,bool>(GameEvent.OnSpellUsed, OnSpellUsed);
        Events.BreakVoidRelay(GameEvent.OnTurnStart, OnTurnStart);
        Events.BreakVoidRelay(ChallengeEvent.OnAttack, OnAttack);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        inUse = true;
        Events.ZipCall(InterfaceEvent.OnSpellSelected, spell, true);
    }

    void OnTurnStart()
    {
        inUse = false;
        Activate();
    }

    void OnAttack()
    {
        if (Pathfinder.AttackCounter == 0) Deactivate();
    }

    void OnSpellUsed(SpellBase spell, bool isStatic)
    {
        if (inUse && spell == this.spell)
        {
            var pathfinder = Pathfinder;
            
            pathfinder.AttackCounter--;
            if (pathfinder.AttackCounter == 0) Deactivate();
        }
        inUse = false;
    }

    private void Activate()
    {
        var color = button.color;
        color.a = 1.0f;
        button.color = color;

        button.raycastTarget = true;
    }
    private void Deactivate()
    {
        var color = button.color;
        color.a = 0.6f;
        button.color = color;

        button.raycastTarget = false;
    }
}