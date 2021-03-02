using System.Collections.Generic;

public class Session
{
    private List<Match> matches = new List<Match>();
    
    //---[Core]---------------------------------------------------------------------------------------------------------/

    public void Add(Match match)
    {
        if (matches.Contains(match)) return;
        
        matches.Add(match);
        match.onEnd += OnMatchEnd;
        
        match.Start();
    }
    
    //---[Callbacks]----------------------------------------------------------------------------------------------------/

    void OnMatchEnd(Match match, Motive motive)
    {
        match.onEnd -= OnMatchEnd;
        matches.Remove(match);
    }
}