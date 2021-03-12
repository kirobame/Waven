using System;
using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Spells", menuName = "Waven/Spells")]
public class Spells : ScriptableObject, IBootable
{
    [SerializeField] private SpellBase[] source;
    [SerializeField] private SpellBase[] mustHave;
    [SerializeField] private SpellBase[] rest;
    [SerializeField] private SpellCategory[] constraints;

    public IReadOnlyDictionary<Id, SpellBase> Registry => registry;
    private Dictionary<Id, SpellBase> registry;

    public IReadOnlyDictionary<SpellKey, List<SpellBase>> All => all;
    public Dictionary<SpellKey, List<SpellBase>> all;

    public void Bootup()
    {
        all = new Dictionary<SpellKey, List<SpellBase>>();
        foreach (var spell in rest) Register(spell);
        foreach (var spell in source) Register(spell);
        
        registry = new Dictionary<Id, SpellBase>();
        foreach (var spell in source.Concat(mustHave))
        {
            if (registry.ContainsKey(spell.Id)) continue;
            registry.Add(spell.Id, spell);
        }
    }
    private void Register(SpellBase spell)
    {
        var key = new SpellKey(spell.Category, spell.Tier);
        
        if (all.TryGetValue(key, out var list)) list.Add(spell);
        else all.Add(key, new List<SpellBase>() { spell });
    }

    public SpellBase GetRandom(SpellKey key)
    {
        while (!all.ContainsKey(key)) key.Downgrade();

        var list = all[key];
        return list[Random.Range(0, list.Count)];
    }
    public IEnumerable<SpellBase> GetDeck(int count)
    {
        if (count < mustHave.Length) count = mustHave.Length;

        var index = 0;
        var deck = new SpellBase[count];
        for (index = 0; index < mustHave.Length; index++) deck[index] = mustHave[index];
        
        source.Shuffle();
        var list = source.ToList();
        
        var limit = Mathf.Min(count - index, constraints.Length);
        for (var i = 0; i < limit; i++)
        {
            var constrainedIndex = list.FindIndex(spell => spell.Category == constraints[i]);
            if (constrainedIndex == -1) continue;

            var foundSpell = list[constrainedIndex];
            list.RemoveAt(constrainedIndex);

            deck[index] = foundSpell;
            index++;
        }

        for (var i = 0; i < count - index; i++) deck[index + i] = source[i];
        
        deck.Shuffle();
        return deck;
    }
}