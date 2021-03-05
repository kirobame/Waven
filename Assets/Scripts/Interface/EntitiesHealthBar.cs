using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public class EntitiesHealthBar : MonoBehaviour
{
    /*public Transform group;
    public GameObject prefab;
    private List<GameObject> prefabList = new List<GameObject>();

    private Text healthText;

    private Vector3 position = new Vector3(-182, 1000, 0);
    [SerializeField] private float heightOffset;

    void Start()
    {
        Events.Open(InterfaceEvent.OnSpellCast);
        Events.RelayByValue<HashSet<Tile>>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
        Events.RelayByValue<SpellBase>(InterfaceEvent.OnSpellCast, OnSpellCast);
        Events.Register(GameEvent.OnTurnStart, OnTurnStart);

        healthText = prefab.transform.GetChild(0).GetComponent<Text>();
    }

    private void OnTurnStart(EventArgs obj)
    {
        foreach (var prefab in prefabList)
        {
            Destroy(prefab);
        }
    }

    private void OnSpellSelected(HashSet<Tile> castZone)
    {
        foreach(Tile tile in castZone)
        {
            if (tile.IsFree()) continue;

            foreach(var entity in tile.Entities)
            {
                if (((Component)entity).name.Contains("Bordermap")) continue;
                if (!entity.TryGet<Damageable>(out Damageable output)) continue;

                var instance = Instantiate(prefab, position, Quaternion.identity, group);
                prefabList.Add(instance);
                
                var life = output.Lives[0];

                prefab.GetComponent<Slider>().value = (float)life.actualValue / life.maxValue;
                healthText.text = $"{life.actualValue} / {life.maxValue}";

                var camera = Repository.Get<Camera>(References.Camera);
                var entityPos = camera.WorldToScreenPoint(output.transform.position+ Vector3.up * heightOffset);

                instance.TryGetComponent<RectTransform>(out RectTransform tr);
                tr.position = entityPos;
            }
        }
    }

    private void OnSpellCast(SpellBase acutalSpell)
    {
        foreach(var prefab in prefabList)
        {
            Destroy(prefab);
            Events.BreakValueRelay<HashSet<Tile>>(InterfaceEvent.OnSpellSelected, OnSpellSelected);
            Events.BreakValueRelay<SpellBase>(InterfaceEvent.OnSpellCast, OnSpellCast);
        }
    }*/
}
