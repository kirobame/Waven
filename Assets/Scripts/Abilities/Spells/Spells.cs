using System.Collections.Generic;
using System.Linq;
using Flux;
using UnityEngine;

[CreateAssetMenu(fileName = "Spells", menuName = "Waven/Spells")]
public class Spells : ScriptableObject, IBootable
{
    [SerializeField] private SpellBase[] source;
    [SerializeField] private SpellBase[] mustHave;
    [SerializeField] private SpellCategory[] constraints;

    public IReadOnlyDictionary<Id, SpellBase> Registry => registry;
    private Dictionary<Id, SpellBase> registry;

    public void Bootup()
    {
        registry = new Dictionary<Id, SpellBase>();
        foreach (var spell in source.Concat(mustHave))
        {
            if (registry.ContainsKey(spell.Id)) continue;
            registry.Add(spell.Id, spell);
        }
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