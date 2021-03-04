using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Match
{
    public event Action<Match, Motive> onEnd;
    
    public Turn Current => turns[index];
    
    private int index;
    private List<Turn> turns = new List<Turn>();
    
    //---[Lifetime]-----------------------------------------------------------------------------------------------------/

    public void Start()
    {
        index = 0;
        Current.Start();
    }
    public void End(Motive motive) => onEnd?.Invoke(this, motive);
    
    //---[Turn handling]------------------------------------------------------------------------------------------------/

    public void Insert(Turn turn)
    {
        if (turns.Contains(turn)) return;
        
        turns.Add(turn);
        Register(turn);
        
        turns.Sort();
        
        var index = turns.IndexOf(turn);
        if (this.index >= index) index++;
    }
    private void Register(Turn turn)
    {
        turn.onEnd += OnTurnEnd;
        turn.Target.Match = this;
    }
    
    public void Remove(ITurnbound target)
    {
        var index = turns.FindIndex(turn => turn.Target == target);
        if (index == -1) return;
        
        turns[index].onEnd -= OnTurnEnd;

        var cachedCurrent = Current;
        turns.RemoveAt(index);
        
        if (this.index == index)
        {
            index--;
            
            var stopMotive = new DeathMotive(target);
            cachedCurrent.Interrupt(stopMotive);

            OnTurnEnd(stopMotive);
        }
        else if (this.index > index) index--;
    }
    
    //---[Callbacks]----------------------------------------------------------------------------------------------------/

    void OnTurnEnd(Motive motive)
    {
        if (!turns.Any())
        {
            End(new IntendedStopMotive());
            return;
        }
        
        if (index + 1 >= turns.Count) index = 0;
        else index++;
        
        Current.Start();
    }
}