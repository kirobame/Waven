using System.Collections.Generic;
using UnityEngine;

public static class Buffer
{
    public static GameObject caster;
    public static bool consumeTriggerSpell;
    
    public static bool isGameTurn;
    
    public static Queue<Vector2Int> slideDirections = new Queue<Vector2Int>();
}