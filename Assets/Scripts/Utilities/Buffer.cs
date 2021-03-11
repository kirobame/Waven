using System.Collections.Generic;
using UnityEngine;

public static class Buffer
{
    public static Navigator caster;
    public static bool consumeTriggerSpell;
    
    public static bool isGameTurn;
    public static bool hasStopped;
    
    public static Queue<Vector2Int> slideDirections = new Queue<Vector2Int>();
}