using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Spells", menuName = "Waven/Spells")]
public class Spells : ScriptableObject
{
    [SerializeField] private SpellBase[] source;
    [SerializeField] private SpellBase[] mustHave;
    [SerializeField] private SpellCategory[] constraints;

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
        return deck;
    }
}