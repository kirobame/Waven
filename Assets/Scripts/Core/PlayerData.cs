using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Flux;
using Flux.Data;
using UnityEngine;

public class PlayerData
{
    public static PlayerData Empty => new PlayerData() { IsValid = false };

    public PlayerData() { }
    public PlayerData(string path, string[] lines)
    {
        IsValid = true;
        Path = path;
        
        deck = new List<SpellBase>();
        if (!lines.Any() || lines.Length == 1 && lines[0] == "Empty") return;

        var spells = Repository.Get<Spells>(References.Spells);
        foreach (var line in lines)
        {
            if (line.Length < 3) continue;

            var id = new Id(line[0], line[1], line[2]);
            if (spells.Registry.TryGetValue(id, out var spell)) deck.Add(spell);
        }
    }

    public bool IsValid { get; private set; }
    public string Path { get; private set; }
    
    public IReadOnlyList<SpellBase> Deck => deck;
    private List<SpellBase> deck;

    public void SetDeck(IEnumerable<SpellBase> spells) => deck = spells.ToList();

    public void Save()
    {
        var lines = new string[deck.Count];
        for (var i = 0; i < deck.Count; i++) lines[i] = deck[i].Id.ToString();
        
        File.WriteAllLines(Path, lines);
    }
}