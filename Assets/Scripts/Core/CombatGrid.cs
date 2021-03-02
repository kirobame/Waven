using Flux.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CombatGrid : MonoBehaviour
{
    private Tilemap map;
    [SerializeField] int range;

    private void Start()
    {
        map = Repository.Get<Tilemap>(References.FeedbackGrid);
    }

    public void DrawGrid(Vector2Int source)
    {
        for (int x = source.x - range; x < source.x + range; x++)
        {
            for (int y = source.y - range; y < source.y + range; y++)
            {
                //var tile = 
            }
        }
    }
}
